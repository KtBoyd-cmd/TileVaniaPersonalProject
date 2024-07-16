using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour

{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] int score = 0;
    [SerializeField] GameObject gameOver;
    [SerializeField] float deathTimer = 1.5f;



    
    public void Awake()
    {
        gameOver.SetActive(false);
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        int numAudioManager = FindObjectsOfType<AudioManager>().Length;
        if (numAudioManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }


    }


    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();

    }

    
    public void ProcessPlayerDeath()

    {
        if (playerLives > 1)
        {   
            gameOver.SetActive(true);
            Invoke("TakeLife", deathTimer);
            
        }
        else
        {
            gameOver.SetActive(true);
            Invoke("ResetGameSession", deathTimer);
        }

        gameOver.SetActive(false);
        
    }
    public void AddToScore(int pointsToAdd)

    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();

    }


}