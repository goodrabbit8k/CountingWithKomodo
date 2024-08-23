using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Tweak Variables")]
    public float enemySpeed = 3f;
    [SerializeField] float forceStrength = 10f;

    [Header("Components Variable")]
    public GameObject enemyNumber;
    Rigidbody enemyRb;
    Animator enemyAnim;
    AudioSource enemyAudio;

    [Header("Math Variables")]  
    public float num1;
    public float num2;
    public char op;
    public float enemyValue;
    public int randomNumber;

    [Header("SFX's Variable")]
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip dead;

    [Header("Reference to another object component")]
    GameObject player;
    Player playerScript;
    GameManager gameManager;

    bool death = false;


    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponentInChildren<Animator>();
        enemyAudio = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
        playerScript = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();

        num1 = Random.Range(1, 10);
        num2 = Random.Range(1, 10);

        if (num1 < num2) 
        {
            num1 = num2 + Random.Range(1,3);
        }

        OperatorChange();
    }

    void Update()
    {
        if (!gameManager.gameOver && !playerScript.blink && !death) 
        {
            FollowPlayer();
            FlipSprite();
        }

        if (SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 8 || SceneManager.GetActiveScene().buildIndex == 12) 
        {
            enemySpeed = 3f;
        }
        else 
        {
            enemySpeed = Random.Range(1.5f, 2.5f);
        }
    }

    void OperatorChange() 
    {
        switch(op) 
        {
            case '+':
                enemyValue = num1 + num2;
                break;
            
            case '-':
                enemyValue = num1 - num2;
                break;
            case 'x':
                enemyValue = num1 * num2;
                break;
            case '÷':
                randomNumber = Random.Range(1, 10);
                num1 = Random.Range(1, 10) * randomNumber;
                num2 = num1 / randomNumber;
                enemyValue = num1 / num2;
                break;
        }
    }

    void FollowPlayer() 
    {
        Vector3 followPlayerDirection = (player.transform.position - transform.position).normalized;
        enemyRb.velocity = followPlayerDirection * enemySpeed;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && !death)
        {
            Rigidbody anotherEnemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromEnemy = anotherEnemyRb.transform.position - transform.position;

            anotherEnemyRb.AddForce(awayFromEnemy * forceStrength, ForceMode.Impulse);
        }    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && playerScript.playerValue == enemyValue)
        {
            death = true;
            Destroy(gameObject, 0.3f);
            enemyAudio.PlayOneShot(dead, 0.5f);
            enemyAnim.SetTrigger("isDeath");
            enemyNumber.SetActive(false);
            gameManager.enemyAmount--;
        }
    }
}
