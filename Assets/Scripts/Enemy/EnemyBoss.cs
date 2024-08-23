using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] GameObject enemyBossNumber;
    [SerializeField] float enemyBossSpeed = 1;
    [SerializeField] RuntimeAnimatorController enemyBossPlusColor;
    [SerializeField] RuntimeAnimatorController enemyBossMinusColor;
    [SerializeField] RuntimeAnimatorController enemyBossMultiColor;
    [SerializeField] RuntimeAnimatorController enemyBossDivideColor;

    [SerializeField] AudioClip hurt;
    [SerializeField] AudioClip dead;

    public int hit;

    public float enemyBossNum1;
    public float enemyBossNum2;
    public int opNumber;
    public char op;
    public float enemyBossValue;

    public int randomNumber;

    Rigidbody enemyBossRb;

    Player playerScript;
    GameObject player;
    GameManager gameManager;
    SpriteRenderer enemyBossColor;
    Animator enemyBossAnim;
    AudioSource enemyBossAudio;

    void Start()
    {
        enemyBossRb = GetComponent<Rigidbody>();
        enemyBossAnim = GetComponentInChildren<Animator>();
        enemyBossAudio = GetComponent<AudioSource>();

        playerScript = FindObjectOfType<Player>();
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();
        enemyBossColor = GetComponentInChildren<SpriteRenderer>();

        enemyBossNum1 = Random.Range(1, 10);
        enemyBossNum2 = Random.Range(1, 10);
        opNumber = Random.Range(1, 3);

        if (SceneManager.GetActiveScene().buildIndex == 8) 
        {
            opNumber = Random.Range(1, 4);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 12) 
        {
            opNumber = Random.Range(1, 5);
        }

        if (enemyBossNum1 < enemyBossNum2)
        {   
            enemyBossNum1 = enemyBossNum2 + Random.Range(1, 3);
        }

        SwitchOperator();
    }

    void Update()
    {
        if (!gameManager.gameOver && !playerScript.blink)
        {
            FollowPlayer();
            FlipSprite();
        }
    }

    void FollowPlayer()
    {
        Vector3 followPlayerDirection = (player.transform.position - transform.position).normalized;
        enemyBossRb.velocity = followPlayerDirection * enemyBossSpeed;
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

    void SwitchOperator() 
    {
        switch(opNumber) 
            {
                case 1:
                    op = '+';
                    enemyBossValue = enemyBossNum1 + enemyBossNum2;
                    enemyBossAnim.runtimeAnimatorController = enemyBossPlusColor;
                    break;
                case 2:
                    op = '-';
                    enemyBossValue = enemyBossNum1 - enemyBossNum2;
                    enemyBossAnim.runtimeAnimatorController = enemyBossMinusColor;
                    break;
                case 3:
                    op = 'x';
                    enemyBossValue = enemyBossNum1 * enemyBossNum2;
                    enemyBossAnim.runtimeAnimatorController = enemyBossMultiColor;
                    break;
                case 4:
                    op = '÷';
                    randomNumber = Random.Range(1, 10);
                    enemyBossNum1 = Random.Range(1, 10) * randomNumber;
                    enemyBossNum2 = enemyBossNum1 / randomNumber;
                    enemyBossValue = enemyBossNum1 / enemyBossNum2;
                    enemyBossAnim.runtimeAnimatorController = enemyBossDivideColor;
                    break;
            }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Bullet" && playerScript.playerValue == enemyBossValue && hit < 5)
        {
            enemyBossNum1 = Random.Range(1, 10);
            enemyBossNum2 = Random.Range(1, 10);
            opNumber = Random.Range(1, 3);

            if (SceneManager.GetActiveScene().buildIndex == 8) 
            {
                opNumber = Random.Range(1, 4);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 12) 
            {
                opNumber = Random.Range(1, 5);
            }

            if (enemyBossNum1 < enemyBossNum2)
            {   
                enemyBossNum1 = enemyBossNum2 + Random.Range(1, 3);
            }

            SwitchOperator();

            Destroy(other.gameObject);

            hit += 1;
            enemyBossAudio.PlayOneShot(hurt, 0.5f);

            if (hit == 5) 
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
                enemyBossAudio.PlayOneShot(dead, 0.5f);
                enemyBossNumber.SetActive(false);
                gameManager.enemyAmount--;
            }
        }   
    }
}
