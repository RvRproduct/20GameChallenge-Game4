using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    static public UIManager Instance;
    [SerializeField] private Image LifeOne;
    [SerializeField] private Image LifeTwo;
    [SerializeField] private Image LifeThree;

    [SerializeField] private AudioClip soTastyClip;
    [SerializeField] private AudioClip burgerKingSongClip;
    [SerializeField] private AudioClip creepySongClip;
    [SerializeField] private AudioClip footLettuceClip;
    [SerializeField] private AudioClip yeahClip;
    [SerializeField] private AudioClip baseMusicClip;

    [SerializeField] TextMeshProUGUI scoreNumber;
    [SerializeField] TextMeshProUGUI highscoreNumber;

    [SerializeField] GameObject LoseBackground;
    [SerializeField] GameObject FootlettuceBackground;
    [SerializeField] GameObject EatButton;

    private int localScore = 0;

    [SerializeField] private PlayerController Player;
    [SerializeField] private AudioSource theSounder;
    [SerializeField] private AudioSource theMusicer;

    // LEVELS
    public const string loseScreen = "LoseScreen";
    public const string mainGame = "MainGame";
    public const string mainMenu = "MainMenu";

    // SOUNDS
    public const string soTasty = "soTasty";
    public const string burgerKingSong = "burgerKingSong";
    public const string creepySong = "creepySong";
    public const string footLettuce = "footLettuce";
    public const string yeahDeadRising = "yeah";
    public const string baseMusic = "baseMusic";
    public const string death = "death";
    public const string destroyed = "destroyed";
    public const string shotPlayer = "shotPlayer";
    public const string shotEnemy = "shotEnemy";

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

        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
        }

        if (highscoreNumber)
        {
            highscoreNumber.text = PlayerPrefs.GetInt("score").ToString();
        }

        if (scoreNumber)
        {
            scoreNumber.text = localScore.ToString();
        }
    }

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case loseScreen:
                theMusicer.Stop();
                theMusicer.clip = creepySongClip;
                theMusicer.Play();
                break;
            case mainGame:
                theMusicer.Stop();
                theMusicer.clip = baseMusicClip;
                theMusicer.Play();
                break;
            case mainMenu:
                theMusicer.Stop();
                theMusicer.clip = baseMusicClip;
                theMusicer.Play();
                break;
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
                Player.PlayerDead();
                SetHighScore();
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

    public int GetLocalScore()
    {
        return localScore;
    }

    public void SetLocalScore(int _score)
    {
        localScore = _score;
        if (scoreNumber)
        {
            scoreNumber.text = localScore.ToString();
        }
    }

    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            return PlayerPrefs.GetInt("score");
        }

        return 0;
    }

    public void SetHighScore()
    {
        if (PlayerPrefs.HasKey("score"))
        {
            if (localScore > PlayerPrefs.GetInt("score"))
            {
                PlayerPrefs.SetInt("score", localScore);
            }
         
            if (highscoreNumber)
            {
                highscoreNumber.text = PlayerPrefs.GetInt("score").ToString();
            }
        }
    }

    public GameObject GetPlayer()
    {
        if (Player)
        {
            return Player.gameObject;
        }

        return null;
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case soTasty:
                theSounder.clip = soTastyClip;
                if (!theSounder.isPlaying)
                {
                    theMusicer.Stop();
                    theSounder.Play();
                    StartCoroutine(RestartGame());
                    return;
                }
                break;
            case footLettuce:
                theSounder.clip = footLettuceClip;
                if (!theSounder.isPlaying)
                {
                    theMusicer.Stop();
                    theSounder.Play();
                    if (LoseBackground)
                    {
                        LoseBackground.SetActive(false);
                        if (FootlettuceBackground)
                        {
                            FootlettuceBackground.SetActive(true);
                        }
                    }
                    StartCoroutine(RestartGame());
                    return;
                }
                break;
            case burgerKingSong:
                theSounder.clip = yeahClip;
                if (!theSounder.isPlaying)
                {
                    theMusicer.Stop();
                    theSounder.Play();
                    LoseBackground?.SetActive(false);
                    EatButton?.SetActive(true);
                    theMusicer.clip = burgerKingSongClip;
                    theMusicer.Play();
                    return;
                }
                break;
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitUntil(() => !theSounder.isPlaying);

        LoadLevel(mainMenu);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
