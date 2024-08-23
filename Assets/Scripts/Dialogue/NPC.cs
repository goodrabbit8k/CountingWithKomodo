using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;

    public Dialogue dialogue;

    Player player;  

    void Start() 
    {
        player = FindObjectOfType<Player>();
    }

    public void NPCInteract() 
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueBox.SetActive(true);
            player.canAttack = false;
        }     
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueBox.SetActive(false);
            player.canAttack = true;
        }
    }
}
