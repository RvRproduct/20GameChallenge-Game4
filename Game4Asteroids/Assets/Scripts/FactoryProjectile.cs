using PoolTags;
using UnityEngine;

public class FactoryProjectile : Factory
{
    public static FactoryProjectile Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetUpObjectPool();
    }

    public override IProduct GetProduct(string poolTag, Vector3 position, Quaternion rotation)
    {
        GameObject projectileInstance = GetValidObjectInPool(poolTag, position, rotation);

        switch (poolTag)
        {
            case PoolTags.ProjectileTags.NormalProjectile:
                Projectile normalProjectile = projectileInstance.GetComponent<Projectile>();

                normalProjectile.Initialize();
                return normalProjectile;
            case PoolTags.ProjectileTags.EnemyProjectile:
                ProjectileEnemy enemyProjectile = projectileInstance.GetComponent<ProjectileEnemy>();

                enemyProjectile.Initialize();
                return enemyProjectile;
        }

        return null;
    }
}
