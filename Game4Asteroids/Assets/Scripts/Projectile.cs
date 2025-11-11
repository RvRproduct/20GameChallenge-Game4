using UnityEngine;

public class Projectile : BasePoolObject, IProduct
{
    private Transform projectile;
    [SerializeField] private float maxLifeTime = 3.0f;
    private float currentLifeTime = 0.0f;
    [SerializeField] private float speed = 5.0f;

    public void Initialize()
    {
        currentLifeTime = 0.0f;
        // Play effects
    }

    protected override string ProvidePoolReturnTag()
    {
        return PoolTags.ProjectileReturnTags.ProjectileReturn;
    }

    protected override string ProvidePoolTag()
    {
        return PoolTags.ProjectileTags.NormalProjectile;
    }

    protected override void Awake()
    {
        base.Awake();
        projectile = GetComponent<Transform>();
    }

    private void Update()
    {
        projectile.position += projectile.up * speed * Time.deltaTime;

        ProjectileLifeTime();
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
