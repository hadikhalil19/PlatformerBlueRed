using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives;
    [SerializeField] int playerScore = 0;


    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start() 
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }
    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }

    }

    void TakeLife()
    {
        playerLives = playerLives - 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void IncreaseScore(int pointsToAdd)
    {
        playerScore = playerScore + pointsToAdd;
        scoreText.text = playerScore.ToString();
    }
}
