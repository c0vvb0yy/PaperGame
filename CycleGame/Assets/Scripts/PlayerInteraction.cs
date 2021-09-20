using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerInteraction : MonoBehaviour
{
    public bool NearNPC;
    DialogueRunner DialogueRunner;
    DialogueUI DialogueUI;
    PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.IsTalking)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {    
                DialogueUI.MarkLineComplete();
            }
        }
        
        if(Input.GetKeyDown(KeyCode.C) && !PlayerManager.IsTalking)
        {
            
            if(NearNPC)
            {
                DialogueRunner.StartDialogue();
            }
            else
            {
                DialogueRunner = GameObject.FindGameObjectWithTag("Anthra").GetComponentInChildren<DialogueRunner>();
                DialogueUI = GameObject.FindGameObjectWithTag("Anthra").GetComponentInChildren<DialogueUI>();
                DialogueRunner.StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("NPC"))
        {
            NearNPC = true;
            DialogueRunner = other.gameObject.GetComponentInChildren<DialogueRunner>();
            DialogueUI = other.gameObject.GetComponentInChildren<DialogueUI>();
        }

        if(other.CompareTag("Item"))
        {
            var item = other.GetComponent<Item>();
            if(item)
            {
                playerManager.Inventory.AddItem(item.HeldItem, 1);
                Destroy(other.gameObject);
            }
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("NPC"))
        {
            NearNPC = false;
            DialogueRunner = null; //maybe a check to see which dialogue exactly in case of two overlapping npc colliders?
            DialogueUI = null;
        }
    }
}
