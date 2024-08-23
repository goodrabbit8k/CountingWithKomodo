using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyNumber : MonoBehaviour
{
    public Vector3 offset;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceUI;
    TextMeshProUGUI enemyNumberText;

    Enemy enemy;

    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceUI =  GameObject.FindGameObjectWithTag("World Space").transform;
        enemyNumberText = GetComponent<TextMeshProUGUI>();

        transform.SetParent(worldSpaceUI);
        enemy = FindObjectOfType<Enemy>();

        offset.y = Random.Range(1f, 2f);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;

        enemyNumberText.text = enemy.num1 + "  " + enemy.op + "  " +  enemy.num2;

        if (FindObjectOfType<GameManager>().gameOver)
        {
            Destroy(gameObject);
        }       
    }
}
