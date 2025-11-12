using UnityEngine;

public class ProjectileEnemy : BasePoolObject, IProduct
{
    private Transform projectile;
    [SerializeField] private float maxLifeTime = 3.0f;
    private float currentLifeTime = 0.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float wrapBuffer = 0.05f;
    private BoxCollider2D boxCollider;

    public void Initialize()
    {
        currentLifeTime = 0.0f;
        boxCollider.enabled = true;
        // Play effects
    }

    protected override string ProvidePoolReturnTag()
    {
        return PoolTags.ProjectileReturnTags.ProjectileReturn;
    }

    protected override string ProvidePoolTag()
    {
        return PoolTags.ProjectileTags.EnemyProjectile;
    }

    protected override void Awake()
    {
        base.Awake();
        projectile = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        projectile.position += projectile.up * speed * Time.deltaTime;
        ProjectileLifeTime();
        WrapAround();
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

    void ProjectileLifeTime()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= maxLifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
