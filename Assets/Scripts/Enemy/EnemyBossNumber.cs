using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBossNumber : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceUI;
    TextMeshProUGUI enemyBossText;

    EnemyBoss enemyBoss;

    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceUI = GameObject.FindGameObjectWithTag("World Space").transform;
        enemyBossText = GetComponent<TextMeshProUGUI>();

        transform.SetParent(worldSpaceUI);
        enemyBoss = FindObjectOfType<EnemyBoss>();
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;

        enemyBossText.text = enemyBoss.enemyBossNum1 + "  " + enemyBoss.op + "  " +  enemyBoss.enemyBossNum2;

        if (FindObjectOfType<GameManager>().gameOver)
        {
            Destroy(gameObject);
        }  
    }
}
