using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkillCard : MonoBehaviour
{
    public bool immune = false;

    [SerializeField] Animator iconAnim;
    [SerializeField] Animator effectAnim;

    [SerializeField] AudioClip skillSound;

    [SerializeField] int healCooldownTime = 3;
    [SerializeField] int armorCooldownTime = 6;
    [SerializeField] int boostCooldownTime = 6;
    [SerializeField] int ultimateCooldownTime = 10;
    [SerializeField] int boostEffectTime = 3;
    [SerializeField] int armorEffectTime = 3;
    [SerializeField] GameObject healCard;
    [SerializeField] GameObject boostCard;
    [SerializeField] GameObject armorCard;
    [SerializeField] GameObject ultimateCard;


    [SerializeField] TextMeshProUGUI healCooldownCountdown;
    [SerializeField] GameObject healBlank;
    float healCooldownCurrentTime = 20f;

    [SerializeField] TextMeshProUGUI boostCooldownCountdown;
    [SerializeField] GameObject boostBlank;
    float boostCooldownCurrentTime = 10f;

    [SerializeField] TextMeshProUGUI armorCooldownCountdown;
    [SerializeField] GameObject armorBlank;
    float armorCooldownCurrentTime = 20f;

    [SerializeField] GameObject ultimateBlank;
    float ultimateCooldownCurrentTime = 5f;


    public int ultimateUsed = 2;

    float playerBoostSpeed = 6f;
    int playerArmorHealth = 2;
    public int ultimateCount = 0;
    float playerNormalSpeed;
    int playerNormalHealth;
    bool canHeal = true;
    bool canBoost = true;
    bool canArmor = true;
    bool canUltimate = true;

    public bool canUseHeal = false;
    public bool canUseBoost = false;
    public bool canUseArmor = false;
    public bool canUseUltimate = false;

    Player player;
    Animator playerAnim;
    Enemy enemy;
    TrainingEnemy trainingEnemy;
    EnemyBoss enemyBoss;

    AudioSource skillAudio;

    void Start() 
    {
        player = GetComponent<Player>();
        playerAnim = gameObject.GetComponentInChildren<Animator>();

        skillAudio = GetComponent<AudioSource>();
        
        playerNormalSpeed = player.runSpeed;
        playerNormalHealth = player.playerHealth;
    }

    void Update() 
    {
        enemy = FindObjectOfType<Enemy>();
        trainingEnemy = FindObjectOfType<TrainingEnemy>();
        enemyBoss = FindObjectOfType<EnemyBoss>();

        if (SceneManager.GetActiveScene().buildIndex == 2) 
        {
            healCard.SetActive(true);
            canUseHeal = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3) 
        {
            healCard.SetActive(true);
            canUseHeal = true;

            boostCard.SetActive(true);
            canUseBoost = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4) 
        {
            healCard.SetActive(true);
            canUseHeal = true;

            boostCard.SetActive(true);
            canUseBoost = true;

            armorCard.SetActive(true);
            canUseArmor = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 5) 
        {
            CanUseAllCard();
        }


        if (canHeal == false) 
        {
            healCooldownCurrentTime -= 1 * Time.deltaTime;
            healCooldownCountdown.text = healCooldownCurrentTime.ToString("0");

            if (healCooldownCurrentTime <= 0) 
            {
                healCooldownCurrentTime = 20f;
            }
        }
        if (canBoost == false)
        {
            boostCooldownCurrentTime -= 1 * Time.deltaTime;
            boostCooldownCountdown.text = boostCooldownCurrentTime.ToString("0");

            if (boostCooldownCurrentTime <= 0) 
            {
                boostCooldownCurrentTime = 10f;
            }
        }
        if (canArmor == false) 
        {
            armorCooldownCurrentTime -= 1 * Time.deltaTime;
            armorCooldownCountdown.text = armorCooldownCurrentTime.ToString("0");

            if (armorCooldownCurrentTime <= 0) 
            {
                armorCooldownCurrentTime = 20f;
            }
        }
        if (canUltimate == false) 
        {
            ultimateCooldownCurrentTime -= 1 * Time.deltaTime;

            if (ultimateCooldownCurrentTime <= 0) 
            {
                ultimateCooldownCurrentTime = 5f;
            }
        }
    }

    void CanUseAllCard() 
    {
        healCard.SetActive(true);
        canUseHeal = true;

        boostCard.SetActive(true);
        canUseBoost = true;

        armorCard.SetActive(true);
        canUseArmor = true;

        ultimateCard.SetActive(true);
        canUseUltimate = true;
    }

    void OnHealing(InputValue value) 
    {
        if (value.isPressed && canHeal && player.playerHealth < playerNormalHealth && !player.death && canUseHeal) 
        {
            Heal();
            iconAnim.SetBool("isHeal", true);
            effectAnim.SetBool("HealEffect", true);
            skillAudio.PlayOneShot(skillSound, 0.8f);
            StartCoroutine(HealIcon());
        }
    }

    IEnumerator HealIcon()
    {
        yield return new WaitForSeconds(1f);
        iconAnim.SetBool("isHeal", false);
        effectAnim.SetBool("HealEffect", false);
    }

    void OnBoosting(InputValue value) 
    {
        if (value.isPressed && canBoost && !player.death && canUseBoost)
        {
            Boost();
            iconAnim.SetBool("isTurbo", true);
            effectAnim.SetBool("BoostEffect", true);
            skillAudio.PlayOneShot(skillSound, 0.8f);
        } 
    } 

    void OnArmor(InputValue value)
    {
        if (value.isPressed && canArmor && !player.death && canUseArmor) 
        {
            Armor();
            iconAnim.SetBool("isArmor", true);
            effectAnim.SetBool("ArmorEffect", true);
            skillAudio.PlayOneShot(skillSound, 0.8f);
        }
    }

    void OnUltimate(InputValue value) 
    {
        if (value.isPressed && canUltimate && !player.death && canUseUltimate) 
        {
            Ultimate();
            iconAnim.SetBool("isUlti", true);
            effectAnim.SetBool("UltiEffect", true);
            skillAudio.PlayOneShot(skillSound, 0.8f);
            StartCoroutine(UltiIcon());
        }
    }

    IEnumerator UltiIcon()
    {
        yield return new WaitForSeconds(1f);
        iconAnim.SetBool("isUlti", false);
        effectAnim.SetBool("UltiEffect", false);
    }

    void Heal() 
    {
        player.playerHealth += 1;
        canHeal = false;
        
        healCooldownCountdown.gameObject.SetActive(true);
        healBlank.SetActive(true);
        StartCoroutine(HealCooldownTimer());
    }

    void Boost() 
    {
        player.runSpeed = playerBoostSpeed;
        playerAnim.SetBool("isBoost", true);

        canBoost = false;

        boostCooldownCountdown.gameObject.SetActive(true);
        boostBlank.SetActive(true);
        StartCoroutine(TurnOffBoostEffect());
        StartCoroutine(BoostCooldownTimer());
    }

    void Armor()
    {
        immune = true;
        canArmor = false;

        armorCooldownCountdown.gameObject.SetActive(true);
        armorBlank.SetActive(true);
        StartCoroutine(TurnOffArmorEffect());
        StartCoroutine(ArmorCooldownTimer());
    }

    void Ultimate() 
    {
        if (trainingEnemy != null && enemy == null && enemyBoss == null) 
        {
            player.playerValue = trainingEnemy.trainingEnemyValue;
        }
        else if (enemy != null && trainingEnemy == null && enemyBoss == null)
        {
            player.playerValue = enemy.enemyValue;
        }
        else if (enemyBoss != null && trainingEnemy == null && enemy == null)
        {
            player.playerValue = enemyBoss.enemyBossValue;
        }   

        ultimateUsed--;
        canUltimate = false;

        if (ultimateUsed != 0) 
        {
            StartCoroutine(UltimateCooldownTimer());
        }
        else if (ultimateUsed == 0)
        {
            ultimateBlank.SetActive(true);
            canUltimate = false;
        }
    }

    IEnumerator HealCooldownTimer() 
    {
        yield return new WaitForSeconds(healCooldownTime);
        canHeal = true;
        healCooldownCountdown.gameObject.SetActive(false);
        healBlank.SetActive(false);
    }

    IEnumerator TurnOffBoostEffect() 
    {
        yield return new WaitForSeconds(boostEffectTime);
        player.runSpeed = playerNormalSpeed;
        playerAnim.SetBool("isBoost", false);
        iconAnim.SetBool("isTurbo", false);
        effectAnim.SetBool("BoostEffect", false);
    }

    IEnumerator BoostCooldownTimer() 
    {
        yield return new WaitForSeconds(boostCooldownTime);
        canBoost = true;
        boostCooldownCountdown.gameObject.SetActive(false);
        boostBlank.SetActive(false);
    }

    IEnumerator TurnOffArmorEffect()
    {
        yield return new WaitForSeconds(armorEffectTime);
        immune = false;
        iconAnim.SetBool("isArmor", false);
        effectAnim.SetBool("ArmorEffect", false);
    }

    IEnumerator ArmorCooldownTimer()
    {
        yield return new WaitForSeconds(armorCooldownTime);
        canArmor = true;
        armorCooldownCountdown.gameObject.SetActive(false);
        armorBlank.SetActive(false);
    }

    IEnumerator UltimateCooldownTimer()
    {
        yield return new WaitForSeconds(ultimateCooldownTime);
        canUltimate = true;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Heal Card Pickup") 
        {
            healCard.SetActive(true);
            canUseHeal = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Boost Card Pickup") 
        {
            boostCard.SetActive(true);
            canUseBoost = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Armor Card Pickup") 
        {
            armorCard.SetActive(true);
            canUseArmor = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Ultimate Card Pickup")
        {
            ultimateCard.SetActive(true);
            canUseUltimate = true;
            Destroy(other.gameObject);
        }
    }
}
