using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Transform player;
    private float speed = 3.0f;
    private PlayerControls playerControls;
    private int currentLives = 3;

    private Vector3 playerDirection = Vector3.zero;
    private Vector3 playerAimPosition;
    private bool isDead = false;

    private float thrustLengthMultiplier = 3.0f;
    private float thrustSpeedMultiplier = 1.5f;
    private Coroutine thrustingCoroutine = null;
    [SerializeField] private float wrapBuffer = 0.05f;
    private ParticleSystem playerParticle;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        player = GetComponent<Transform>();
        playerParticle = GetComponent<ParticleSystem>();
        playerControls = new PlayerControls();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentLives = 3;
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.performed += OnPlayerMove;
        playerControls.Player.Move.canceled += OnPlayerMove;

        playerControls.Player.Aim.performed += OnPlayerAim;

        playerControls.Player.Thrust.started += OnPlayerThrust;

        playerControls.Player.Shoot.started += OnPlayerShoot;
    }

    private void OnDisable()
    {
        playerControls.Player.Move.performed -= OnPlayerMove;
        playerControls.Player.Move.canceled -= OnPlayerMove;

        playerControls.Player.Aim.performed -= OnPlayerAim;

        playerControls.Player.Shoot.started -= OnPlayerShoot;
    }

    private void Update()
    {
        if (!isDead)
        {
            PlayerMovementImplemented();
            WrapAround();
        }    
    }

    public void PlayerDead()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        isDead = true;
        playerParticle.Play();
        StartCoroutine(OnPlayerDestory());

    }

    private IEnumerator OnPlayerDestory()
    {
        yield return new WaitUntil(() => !playerParticle.IsAlive(true));

        gameObject.SetActive(false);
        UIManager.Instance.LoadLevel(UIManager.loseScreen);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.SetActive(false);
            if (currentLives > 0)
            {
                currentLives--;
            }
            UIManager.Instance.SetCurrentLives(currentLives);
        }

        if (collision.gameObject.tag == "ProjectileEnemy")
        {
            collision.gameObject.SetActive(false);
            if (currentLives > 0)
            {
                currentLives--;
            }
            UIManager.Instance.SetCurrentLives(currentLives);
        }
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        Vector3 playerMovement = context.ReadValue<Vector2>();
        playerDirection = playerMovement.normalized;
    }

    private void OnPlayerAim(InputAction.CallbackContext context)
    {
        Vector3 playerAim = context.ReadValue<Vector2>();
        playerAim = Camera.main.ScreenToWorldPoint(playerAim);
        playerAimPosition = playerAim;
        PlayerAimImplemented();
    }

    private void OnPlayerThrust(InputAction.CallbackContext context)
    {
        if (thrustingCoroutine == null)
        {
            StopAllCoroutines();
            thrustingCoroutine = StartCoroutine(PlayerThrustImplemented());
        }
    }
    private void OnPlayerShoot(InputAction.CallbackContext context)
    {
        Vector3 spawnPosition = player.position;
        Quaternion spawnRotation = player.rotation;

        spawnPosition += player.up * 0.5f;

        FactoryProjectile.Instance.GetProduct(PoolTags.ProjectileTags.NormalProjectile, spawnPosition, spawnRotation);
    }

    private void PlayerMovementImplemented()
    {
        if (player && thrustingCoroutine == null)
        {
            player.position += playerDirection * speed * Time.deltaTime;
        }
    }

    private void PlayerAimImplemented()
    {
        if (player)
        {
            Vector3 aimDirection = (playerAimPosition - player.position).normalized;

            float angle = (Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg) * -1.0f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            player.rotation = targetRotation;
        }
    }

    private void WrapAround()
    {
        Vector3 position = transform.position;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        bool wrapped = false;

        if (viewportPosition.x > 1f + wrapBuffer)
        {
            viewportPosition.x = 0f - wrapBuffer;
            wrapped = true;
        }
        else if (viewportPosition.x < 0f - wrapBuffer)
        {
            viewportPosition.x = 1f + wrapBuffer;
            wrapped = true;
        }

        if (viewportPosition.y > 1f + wrapBuffer)
        {
            viewportPosition.y = 0f - wrapBuffer;
            wrapped = true;
        }
        else if (viewportPosition.y < 0f - wrapBuffer)
        {
            viewportPosition.y = 1f + wrapBuffer;
            wrapped = true;
        }

        if (wrapped)
        {
            transform.position = Camera.main.ViewportToWorldPoint(viewportPosition);
        }
    }

    private IEnumerator PlayerThrustImplemented()
    {
        Vector3 startPosition = player.position;
        Vector3 targetPosition = (player.up * thrustLengthMultiplier) + player.position;
        float distanceToTarget = Vector3.Distance(startPosition, targetPosition);
        float moveDuration = ((speed * thrustSpeedMultiplier) > 0.0f) ? distanceToTarget / (speed * thrustSpeedMultiplier) : 0.0f;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / moveDuration;
            player.position = Vector3.Lerp(player.position, targetPosition, t);

            float currentDistanceToTarget = Vector3.Distance(player.position, targetPosition);

            if (currentDistanceToTarget <= 0.01f)
            {
                break;
            }

            yield return null;
        }

        player.position = targetPosition;
        thrustingCoroutine = null;
    }
}
