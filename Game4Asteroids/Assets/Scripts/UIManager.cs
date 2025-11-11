using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    private int numberOfAsteroidsDestroyed = 0;
    [SerializeField] private Image LifeOne;
    [SerializeField] private Image LifeTwo;
    [SerializeField] private Image LifeThree;

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

    public void SetCurrentLives(int currentLives)
    {
        switch (currentLives)
        {
            case 0:
                LifeOne.gameObject.SetActive(false);
                LifeTwo.gameObject.SetActive(false);
                LifeThree.gameObject.SetActive(false);
                break;
            case 1:
                LifeOne.gameObject.SetActive(true);
                LifeTwo.gameObject.SetActive(false);
                LifeThree.gameObject.SetActive(false);
                break;
            case 2:
                LifeOne.gameObject.SetActive(true);
                LifeTwo.gameObject.SetActive(true);
                LifeThree.gameObject.SetActive(false);
                break;
            case 3:
                LifeOne.gameObject.SetActive(true);
                LifeTwo.gameObject.SetActive(true);
                LifeThree.gameObject.SetActive(true);
                break;
        }   
    }

    public int GetNumberOfAsteroidsDestroyed()
    {
        return numberOfAsteroidsDestroyed;
    }

    public void SetNumberOfAsteroidsDestroyed(int _numberOfAsteroidsDestroyed)
    {
        numberOfAsteroidsDestroyed = _numberOfAsteroidsDestroyed;
    }
}
