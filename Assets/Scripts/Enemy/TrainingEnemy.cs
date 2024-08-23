using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingEnemy : MonoBehaviour
{
    public GameObject trainingEnemyNumber;
    public float trainingEnemySpeed = 3f;
    public float trainingNum1;
    public float trainingNum2;
    public int opNumber;
    public char op;
    public float trainingEnemyValue;

    public int haha;
    public int randomNumber;

    GameObject player;
    Player playerScript;
    GameManager gameManager;
    Rigidbody trainingEnemyRb;

    void Start()
    {
        trainingEnemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerScript = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();

        trainingNum1 = Random.Range(1, 10);
        trainingNum2 = Random.Range(1, 10);

        if (SceneManager.GetActiveScene().buildIndex == 1) 
        {
            opNumber = Random.Range(1, 3);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5) 
        {
            opNumber = 3;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 9) 
        {
            opNumber = 4;
        }

        if (trainingNum1 < trainingNum2) 
        {
            trainingNum1 = trainingNum2 + Random.Range(1, 3);
        }

        SwitchOperator();
    }

    void FlipSprite() 
    {
        Vector3 scale = transform.localScale;

        if (player.transform.position.x > transform.position.x) 
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else 
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && playerScript.playerValue == trainingEnemyValue)
        {
            if (opNumber != 4) 
            {
                trainingNum1 = Random.Range(1, 10);
                trainingNum2 = Random.Range(1, 10);
            }
            else 
            {
                randomNumber = Random.Range(1, 10);
                trainingNum1 = Random.Range(1, 10) * randomNumber;
                trainingNum2 = trainingNum1 / randomNumber;
            }
            
            
            if (SceneManager.GetActiveScene().buildIndex == 1) 
            {
                opNumber = Random.Range(1, 3);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 5) 
            {
                opNumber = 3;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 9) 
            {
                opNumber = 4;
            }

            if (trainingNum1 < trainingNum2) 
            {
                trainingNum1 = trainingNum2 + Random.Range(1, 3);
            }

            SwitchOperator();

            Destroy(other.gameObject);
        }   
    }

    void SwitchOperator() 
    {
        switch(opNumber) 
            {
                case 1:
                    op = '+';
                    trainingEnemyValue = trainingNum1 + trainingNum2;
                    break;
                case 2:
                    op = '-';
                    trainingEnemyValue = trainingNum1 - trainingNum2;
                    break;
                case 3:
                    op = 'x';
                    trainingEnemyValue = trainingNum1 * trainingNum2;
                    break;
                case 4:
                    op = '÷';
                    randomNumber = Random.Range(1, 10);
                    trainingNum1 = Random.Range(1, 10) * randomNumber;
                    trainingNum2 = trainingNum1 / randomNumber;
                    trainingEnemyValue = trainingNum1 / trainingNum2;
                    break;
            }
    }
}
