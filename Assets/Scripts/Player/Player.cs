using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float bottomBoundary = 7f;
    [SerializeField] float topBoundary = 20f;
    [SerializeField] float leftBoundary = -8f;
    [SerializeField] float rightBoundary = 30f;
    [SerializeField] GameObject healCardPickup;
    [SerializeField] GameObject boostCardPickup;
    [SerializeField] GameObject armorCardPickup;
    [SerializeField] GameObject ultimateCardPickup;

    [SerializeField] Material defaultFlash;
    [SerializeField] Material blinkFlash;
    [SerializeField] GameObject pausePanel;

    public AudioClip walkSound;
    public AudioClip hurtSound;
    public AudioClip shootSound;
    public AudioClip scrollSound;

    public SpriteRenderer spriteRenderer;
    public GameObject bulletPrefab;
    public Transform gunPos;
    public float runSpeed = 3f;
    public float damageKick = 5f;
    public float attackAnimationTime = 0.5f;
    public float blinkAnimationTime = 1f;
    public bool canMove = true;
    public bool blink = false;
    public bool death = false;
    public bool canAttack = true;
    public int playerHealth;
    public float playerValue;
    public int maxChooseNumber;

    bool healCardPicked;
    bool boostCardPicked;
    bool armorCardPicked;
    bool UltimateCardPicked;

    public bool dialog = false;
    public bool pause = false;

    Rigidbody playerRb;
    Animator playerAnim;
    AudioSource playerAudio;
    GameObject enemy;
    Enemy enemyScript;
    GameObject enemyBoss;
    Vector2 moveInput;
    SkillCard skillCard; 
    GameManager gameManager;   
    DialogueManager dialogueManager;

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();
        playerAudio = gameObject.GetComponent<AudioSource>();
        skillCard = gameObject.GetComponent<SkillCard>();
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = FindObjectOfType<Enemy>();
        enemyBoss = GameObject.FindGameObjectWithTag("Enemy Boss");

        PlayerBoundary();
        
        if (canMove && !blink && !gameManager.gameOver) 
        {
            Run();
            FlipSprite();
        }

        if (!canMove) 
        {
            playerRb.velocity = new Vector3(0, 0, 0);
            playerAnim.SetBool("isRun", false);
        }

        if (playerHealth < 0) 
        {
            playerHealth = 0;
        }

        PlayerDeath();
        IncreaseAndDecrease();
        DoubleIncreaseAndDecrease();
    }

    void PlayerBoundary()
    {
        if (transform.position.z < bottomBoundary)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, bottomBoundary);
        }
        else if (transform.position.z > topBoundary) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, topBoundary);
        }
        else if (transform.position.x < leftBoundary) 
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > rightBoundary) 
        {
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
    }

    void OnMove(InputValue value) 
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector3 playerMovement = new Vector3(moveInput.x * runSpeed, 0f, moveInput.y * runSpeed);
        playerRb.velocity = playerMovement;

        playerAnim.SetBool("isRun", true);

        if (playerMovement == new Vector3(0,0)) 
        {
            playerAnim.SetBool("isRun", false);
        }
    }

    void FlipSprite() 
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    IEnumerator TurnOffAttackAnimation() 
    {
        yield return new WaitForSeconds(attackAnimationTime);
        playerAnim.SetBool("isAttack", false);
        canAttack = true;
    }

    void OnFire(InputValue value) 
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (value.isPressed && !playerHasHorizontalSpeed && canAttack && !death) 
        {
            Attack();
            canAttack = false;
            StartCoroutine(TurnOffAttackAnimation());
        }
    }

    void Attack() 
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        Instantiate(bulletPrefab, gunPos.position, transform.rotation);
        playerAudio.PlayOneShot(shootSound, 0.5f);

        if (!playerHasHorizontalSpeed)
        {
            playerAnim.SetBool("isAttack", true);
        }
        else
        {
            playerAnim.SetBool("isAttack", false);
        }
    }

    IEnumerator TurnOffBlinkAnimation() 
    {
        yield return new WaitForSeconds(blinkAnimationTime);
        blink = false;
        GetComponentInChildren<SpriteRenderer>().material = defaultFlash;
        playerAnim.SetBool("isBlink", false);
    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Enemy Boss" && !blink) 
        {
            if (enemy == null) 
            {
                if (transform.position.x > enemyBoss.transform.position.x) 
                {
                    playerRb.AddForce(Vector3.right * damageKick, ForceMode.VelocityChange);
                }
                else 
                {
                    playerRb.AddForce(Vector3.left * damageKick, ForceMode.VelocityChange);
                }
            }
            else 
            {
                if (transform.position.x > enemy.transform.position.x) 
                {
                    playerRb.AddForce(Vector3.right * damageKick, ForceMode.VelocityChange);
                }
                else 
                {
                    playerRb.AddForce(Vector3.left * damageKick, ForceMode.VelocityChange);
                }
            }

            if (other.gameObject.tag == "Enemy" && !skillCard.immune)
            {
                playerHealth -= 1;
            }
            else if (other.gameObject.tag == "Enemy Boss" && !skillCard.immune)
            {
                playerHealth -= 2;
            }

            blink = true;
            playerAudio.PlayOneShot(hurtSound, 0.5f);
            playerAnim.SetBool("isBlink", true);
            GetComponentInChildren<SpriteRenderer>().material = blinkFlash;
            StartCoroutine(TurnOffBlinkAnimation());
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "NPC")
        {
            PlayerInteract();
        }
        else if (other.gameObject.tag == "Professor Heal Card") 
        {
            PlayerInteract();

            if (!healCardPicked) 
            {
                healCardPickup.SetActive(true);
                healCardPicked = true;
            }
        }
        else if (other.gameObject.tag == "Professor Boost Card") 
        {
            PlayerInteract();

            if (!boostCardPicked)
            {
                boostCardPickup.SetActive(true);
                boostCardPicked = true;
            }
        }
        else if (other.gameObject.tag == "Professor Armor Card") 
        {
            PlayerInteract();

            if (!armorCardPicked)
            {
                armorCardPickup.SetActive(true);
                armorCardPicked = true;
            }
        }
        else if (other.gameObject.tag == "Professor Ultimate Card") 
        {
            PlayerInteract();

            if (!UltimateCardPicked)
            {
                ultimateCardPickup.SetActive(true);
                UltimateCardPicked = true;
            }
        }
    }

    void IncreaseAndDecrease()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && playerValue < maxChooseNumber)  
        {
            playerValue += 1;
            playerAudio.PlayOneShot(scrollSound, 0.5f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && playerValue > 0) 
        {
            playerValue -= 1;
            playerAudio.PlayOneShot(scrollSound, 0.5f);
        }
    }

    void DoubleIncreaseAndDecrease()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerValue < maxChooseNumber)  
        {
            playerValue += 10;
            playerAudio.PlayOneShot(scrollSound, 0.5f);

            if (playerValue >= maxChooseNumber) 
            {
                playerValue = maxChooseNumber;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && playerValue > 0) 
        {
            playerValue -= 10;
            playerAudio.PlayOneShot(scrollSound, 0.5f);

            if (playerValue <= 0f) 
            {
                playerValue = 0f;
            }
        }
    }

    void PlayerDeath() 
    {
        if (playerHealth <= 0) 
        {
            death = true;
            canMove = false;
            playerAnim.SetTrigger("isDeath");
            gameManager.GameOver();
        }
    }

    void PlayerInteract()
    {
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) 
        {
            if (collider.TryGetComponent(out NPC npcInteract)) 
            {
                npcInteract.NPCInteract();
                canMove = false;
                dialog = true;
            }
        }
    }

    public void CantAttackPause(bool value) => canAttack = value;
}
