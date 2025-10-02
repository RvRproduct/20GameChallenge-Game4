using UnityEngine;

public class FactoryProjectile : Factory
{
    public static FactoryProjectile Instance;

    [SerializeField] Projectile projectile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override IProduct GetProduct(Vector3 position, Quaternion rotation)
    {
        GameObject projectileInstance = Instantiate(projectile.gameObject,
            position, rotation);

        Projectile newProjectile = projectileInstance.GetComponent<Projectile>();

        newProjectile.Initialize();

        return newProjectile;
    }
}
