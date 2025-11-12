using PoolTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saucer : BasePoolObject, IProduct
{
    [SerializeField] private float speed = 3.0f;
    private float currentSpeed = 3.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float maxShootRate = 3.0f;
    private float currentShootRate = 0.0f;
    private Transform saucer;
    private CircleCollider2D circleCollider;
    [SerializeField] private float wrapBuffer = 0.05f;
    private Vector3 saucerDirection = new Vector3();
    private ParticleSystem saucerParticle;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        saucerParticle = GetComponent<ParticleSystem>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        saucer = GetComponent<Transform>();
    }
    public void Initialize()
    {
        circleCollider.enabled = true;
        spriteRenderer.enabled = true;
        RandomizeDirection();
        RandomizeSpeed();
        currentSpeed = speed;
        currentShootRate = 0.0f;
    }

    protected override string ProvidePoolReturnTag()
    {
        return PoolTags.SaucerReturnTags.SaucerReturn;
    }

    protected override string ProvidePoolTag()
    {
        return PoolTags.SaucerTags.NormalSaucer;
    }

    private void Update()
    {
        saucer.position += saucerDirection * currentSpeed * Time.deltaTime;
        if (UIManager.Instance.GetPlayer())
        {
            RotateToPlayer();
        }
        SaucerShoot();
        WrapAround();
    }

    private void SaucerShoot()
    {
        currentShootRate += Time.deltaTime;
        if (currentShootRate >= maxShootRate)
        {
            Vector3 spawnPosition = saucer.position;
            Quaternion spawnRotation = saucer.rotation * Quaternion.Euler(0f, 0f, 180f);

            spawnPosition += (saucer.up * -0.5f);

            FactoryProjectile.Instance.GetProduct(PoolTags.ProjectileTags.EnemyProjectile, spawnPosition, spawnRotation);
            currentShootRate = 0.0f;
        }
    }

    private void RandomizeDirection()
    {
        float xDirection;
        float yDirection;
        xDirection = Random.Range(-1f, 1f);
        yDirection = Random.Range(-1f, 1f);

        saucerDirection.x = xDirection;
        saucerDirection.y = yDirection;
        saucerDirection.z = 1.0f;
    }

    private void RandomizeSpeed()
    {
        speed = Random.Range(3f, 5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            collision.gameObject.SetActive(false);
            circleCollider.enabled = false;
            currentSpeed = 0.0f;
            spriteRenderer.enabled = false;
            UIManager.Instance.SetLocalScore(UIManager.Instance.GetLocalScore() + 1);
            UIManager.Instance.SetHighScore();
            saucerParticle.Play();
            StartCoroutine(OnSaucerDestory());
        }
    }

    private IEnumerator OnSaucerDestory()
    {
        yield return new WaitUntil(() => !saucerParticle.IsAlive(true));

        gameObject.SetActive(false);
    }

    private void RotateToPlayer()
    {
        Vector3 direction = UIManager.Instance.GetPlayer().transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Magic no it has to do with how the sprite is facing
        targetAngle += 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
            );
    }
}
