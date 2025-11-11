using PoolTags;
using UnityEngine;

public class FactoryAsteroid : Factory
{
    public static FactoryAsteroid Instance;

    [SerializeField] private float spawnMargin = 0.1f;
    [SerializeField] private float maxSpawnRate = 3.0f;
    private float currentSpawnRate = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        currentSpawnRate = maxSpawnRate;
    }

    private void Start()
    {
        SetUpObjectPool();
    }

    private void Update()
    {
        currentSpawnRate += Time.deltaTime;

        if (currentSpawnRate >= maxSpawnRate)
        {
            GetProduct(SpawnOffscreen(), Quaternion.identity);
            currentSpawnRate = 0.0f;
        }
    }

    public override IProduct GetProduct(Vector3 position, Quaternion rotation)
    {
        GameObject asteroidInstance = GetValidObjectInPool(AsteroidTags.NormalAsteroid, position, rotation);

        if (asteroidInstance == null) { return null; }

        Asteroid newAsteroid = asteroidInstance.GetComponent<Asteroid>();

        newAsteroid.Initialize();

        return newAsteroid;
    }

    private Vector3 SpawnOffscreen()
    {
        int edge = Random.Range(0, 4);

        Vector3 viewportPosition = Vector3.zero;

        switch (edge)
        {
            case 0:
                viewportPosition.x = -spawnMargin;
                viewportPosition.y = Random.value;
                break;
            case 1:
                viewportPosition.x = spawnMargin;
                viewportPosition.y = Random.value;
                break;
            case 2:
                viewportPosition.x = Random.value;
                viewportPosition.y = -spawnMargin;
                break;
            case 3:
                viewportPosition.x = Random.value;
                viewportPosition.y = spawnMargin;
                break;
        }

        float zDist = Mathf.Abs(Camera.main.transform.position.z);
        viewportPosition.z = zDist;

        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        return worldPosition;
    }
}
