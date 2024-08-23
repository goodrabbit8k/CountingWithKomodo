using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyBoss;
    [SerializeField] int startGameTime = 5;
    [SerializeField] int enemiesWave = 1;

    public GameObject[] enemyPrefab;
    public int enemyCount;
    public bool startGame = false;

    bool canSpawn = false;

    GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(StartSpawnCountdown());
    }

    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount < enemiesWave && startGame && !gameManager.levelComplete && canSpawn) 
        {
            int randomEnemy = Random.Range(0, 2);

            if (SceneManager.GetActiveScene().buildIndex > 5) 
            {
                randomEnemy = Random.Range(0 ,3);
            }
            else if (SceneManager.GetActiveScene().buildIndex > 9) 
            {
                randomEnemy = Random.Range(0, 4);
            }

            Instantiate(enemyPrefab[randomEnemy], GenerateRandomSpawnPosition(), enemyPrefab[randomEnemy].transform.rotation);
        
            canSpawn = false;
        }

        if (enemyCount == 0) 
        {
            canSpawn = true;
        }

        if (gameManager.enemyAmount == 1 && SceneManager.GetActiveScene().buildIndex == 4 || gameManager.enemyAmount == 1 && SceneManager.GetActiveScene().buildIndex == 8 || gameManager.enemyAmount == 1 && SceneManager.GetActiveScene().buildIndex == 12)
        {
            canSpawn = false;
            enemyBoss.SetActive(true);
        }
    }

    IEnumerator StartSpawnCountdown() 
    {
        yield return new WaitForSeconds(startGameTime);
        startGame = true;
        canSpawn = true;
    }

    Vector3 GenerateRandomSpawnPosition() 
    {
        float spawnRandomX = Random.Range(10f, 30f);
        float spawnRandomZ = Random.Range(8f, 15f);

        Vector3 spawnPos = new Vector3(spawnRandomX, 0.405f, spawnRandomZ);

        return spawnPos;
    }
}
