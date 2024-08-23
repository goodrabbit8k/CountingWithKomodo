using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float loadLevelDelay = 2f;

    Player player;
    GameManager gameManager;

    void Start() 
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();    
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player" && gameManager.levelComplete) 
        {
            player.canMove = false;
            StartCoroutine(LoadNextLevel());
        }    
    }

    IEnumerator LoadNextLevel() 
    {
        yield return new WaitForSecondsRealtime(loadLevelDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
