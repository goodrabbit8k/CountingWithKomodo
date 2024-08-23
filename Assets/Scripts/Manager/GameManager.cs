using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerHealthBar;
    [SerializeField] TextMeshProUGUI enemyLeftText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject professorBoostCard;
    [SerializeField] GameObject professorArmorCard;
    [SerializeField] GameObject professorUltimateCard;

    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject completeText;
    [SerializeField] GameObject pauseButton;

    [SerializeField] GameObject timer;

    public int enemyAmount = 10;
    public float timeRemaining = 0;
    public bool timerIsRunning = false;
    public bool levelComplete;
    public bool gameOver;

    Player player;
    SpawnManager spawnManager;

    void Start()
    {
        player = FindObjectOfType<Player>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        playerHealthBar.text = "X " + player.playerHealth.ToString();
        enemyLeftText.text = "X " + enemyAmount.ToString();

        if (spawnManager != null) 
        {
            if (spawnManager.startGame) 
            {
                timerIsRunning = true;
            }
        }

        if (timerIsRunning && !levelComplete && !gameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }

            if (timerIsRunning == false && enemyAmount != 0) 
            {
                GameOver();
            }

            DisplayTime(timeRemaining);
        }

        LevelComplete();
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    void LevelComplete() 
    {
        if (enemyAmount == 0)
        {
            levelComplete = true;

            if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 5 && SceneManager.GetActiveScene().buildIndex != 9) 
            {
                if (!player.dialog) 
                {
                    completeText.SetActive(true);
                }
                else 
                {
                    completeText.SetActive(false);
                }
            }
            
            if (timer != null) 
            {
                timer.SetActive(false);
            }

            if (SceneManager.GetActiveScene().buildIndex == 2) 
            {
                professorBoostCard.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                professorArmorCard.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                professorUltimateCard.SetActive(true);
            }
        }
    }
}
