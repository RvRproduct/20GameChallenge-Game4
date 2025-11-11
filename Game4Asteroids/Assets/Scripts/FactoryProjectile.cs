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

    public override IProduct GetProduct(Vector3 position, Quaternion rotation)
    {
        GameObject projectileInstance = GetValidObjectInPool(ProjectileTags.NormalProjectile, position, rotation);

        Projectile newProjectile = projectileInstance.GetComponent<Projectile>();

        newProjectile.Initialize();

        return newProjectile;
    }
}
