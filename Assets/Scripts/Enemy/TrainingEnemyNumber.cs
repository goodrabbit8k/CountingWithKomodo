using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingEnemyNumber : MonoBehaviour
{
    public Vector3 offset;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceUI;
    TextMeshProUGUI trainingEnemyText;

    TrainingEnemy trainingEnemy;

    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceUI = GameObject.FindGameObjectWithTag("World Space").transform;
        trainingEnemyText = GetComponent<TextMeshProUGUI>();

        transform.SetParent(worldSpaceUI);
        trainingEnemy = FindObjectOfType<TrainingEnemy>();

        offset.y = Random.Range(1f, 2f);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;

        trainingEnemyText.text = trainingEnemy.trainingNum1 + "  " + trainingEnemy.op + "  " +  trainingEnemy.trainingNum2; 

        if (FindObjectOfType<GameManager>().gameOver)
        {
            Destroy(gameObject);
        }        
    }
}
