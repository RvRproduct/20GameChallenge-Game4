using UnityEngine;

public class Projectile : MonoBehaviour, IProduct
{
    private Transform projectile;
    [SerializeField] private float speed = 5.0f;

    public void Initialize()
    {
        // Play effects
    }

    private void Awake()
    {
        projectile = GetComponent<Transform>();
    }

    private void Update()
    {
        projectile.position += projectile.up * speed * Time.deltaTime;
    }
}
