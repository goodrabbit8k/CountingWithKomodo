using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNumber : MonoBehaviour
{
    public Vector3 offset;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceUI; 
    TextMeshProUGUI playerNumberText;
    Player player;

    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        playerNumberText = GetComponent<TextMeshProUGUI>();
        worldSpaceUI =  GameObject.FindGameObjectWithTag("World Space").transform;
        player = FindObjectOfType<Player>();

        transform.SetParent(worldSpaceUI);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;

        playerNumberText.text = player.playerValue.ToString();

        if (FindObjectOfType<GameManager>().gameOver)
        {
            Destroy(gameObject);
        }  
    }
}
