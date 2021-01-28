using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerInteraction : MonoBehaviour
{
    public bool NearNPC;
    public bool isTalking;
    DialogueRunner DialogueRunner;
    DialogueUI DialogueUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTalking)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                DialogueUI.MarkLineComplete();
            }
        }
        
        if(NearNPC && Input.GetKeyDown(KeyCode.C) && !isTalking)
        {
            DialogueRunner.StartDialogue();
            isTalking = true;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("NPC"))
        {
            NearNPC = true;
            DialogueRunner = other.gameObject.GetComponent<DialogueRunner>();
            DialogueUI = other.gameObject.GetComponent<DialogueUI>();
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("NPC"))
        {
            NearNPC = false;
            DialogueRunner = null; //maybe a check to see which dialogue exactly in case of two overlapping npc colliders?
        }
    }

    public void StopTalking()
    {
        isTalking = false;
    }
}
