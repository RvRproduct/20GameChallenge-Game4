using System.Collections.Generic;
using PoolTags;
using UnityEngine;

public class Asteroid : BasePoolObject, IProduct
{
    enum EAsteroidLevel
    {
        LevelOne,
        LevelTwo, 
        LevelThree
    }

    private Transform asteroid;
    private Dictionary<EAsteroidLevel, Vector3> AsteroidScales = new Dictionary<EAsteroidLevel, Vector3>();
    private EAsteroidLevel currentLevel;
    private Vector3 asteroidDirection = new Vector3();
    private float speed = 3.0f;
    private float rotationSpeed = 90.0f;
    [SerializeField] private float wrapBuffer = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        asteroid = GetComponent<Transform>();
        AsteroidScales.Add(EAsteroidLevel.LevelOne, new Vector3(1.0f, 1.0f, 1.0f));
        AsteroidScales.Add(EAsteroidLevel.LevelTwo, new Vector3(2.0f, 2.0f, 2.0f));
        AsteroidScales.Add(EAsteroidLevel.LevelThree, new Vector3(3.0f, 3.0f, 3.0f));
    }

    public void Initialize()
    {
        RandomizeInitialScale();
        RandomizeDirection();
        RandomizeSpeed();
    }

    protected override string ProvidePoolReturnTag()
    {
        return PoolTags.AsteroidReturnTags.AsteroidReturn;
    }

    protected override string ProvidePoolTag()
    {
        return PoolTags.AsteroidTags.NormalAsteroid;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            collision.gameObject.SetActive(false);
            ChangeAsteroidLevel();
        }
    }

    private void Update()
    {
        asteroid.position += asteroidDirection * speed * Time.deltaTime;
        asteroid.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        WrapAround();
    }

    private void RandomizeInitialScale()
    {
        if (AsteroidScales.Count == 0) { return; }

        List<EAsteroidLevel> AllLevels = new List<EAsteroidLevel>();

        foreach(EAsteroidLevel level in AsteroidScales.Keys)
        {
            AllLevels.Add(level);
        }

        int randomIndex = Random.Range(0, AllLevels.Count);
        currentLevel = AllLevels[randomIndex];
        Vector3 initialAsteroidScale = new Vector3(1.5f, 1.5f, 1.5f);
        AsteroidScales.TryGetValue(currentLevel, out initialAsteroidScale);
        gameObject.transform.localScale = initialAsteroidScale;
    }

    private void RandomizeDirection()
    {
        float xDirection;
        float yDirection;
        xDirection = Random.Range(-1f, 1f);
        yDirection = Random.Range(-1f, 1f);

        asteroidDirection.x = xDirection;
        asteroidDirection.y = yDirection;
        asteroidDirection.z = 1.0f;
    }

    private void RandomizeSpeed()
    {
        speed = Random.Range(3f, 8f);
        rotationSpeed = Random.Range(90f, 180f);
        float coinFlip = Random.Range(0f, 1f);

        if (coinFlip >= 0.5f)
        {
            rotationSpeed *= -1.0f;
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

    private void ChangeAsteroidLevel()
    {
        switch (currentLevel)
        {
            case EAsteroidLevel.LevelOne:
                gameObject.SetActive(false);
                break;
            case EAsteroidLevel.LevelTwo:
                currentLevel = EAsteroidLevel.LevelOne;
                UpdateAsteroidScale();
                break;
            case EAsteroidLevel.LevelThree:
                currentLevel = EAsteroidLevel.LevelTwo;
                UpdateAsteroidScale();
                break;
        }
    }

    private void UpdateAsteroidScale()
    {
        Vector3 initialAsteroidScale = new Vector3(1.5f, 1.5f, 1.5f);
        AsteroidScales.TryGetValue(currentLevel, out initialAsteroidScale);
        gameObject.transform.localScale = initialAsteroidScale;
    }
}
