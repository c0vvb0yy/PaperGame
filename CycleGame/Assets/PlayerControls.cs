using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject InventoryCanvas;
    public GameObject PartyStatsCanvas;
    
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PlayerManager.PauseGame();
                DisplayPauseMenu();
            }
            else
            {
                PlayerManager.ResumeGame();
                HidePauseMenu();
            }
            isPaused = !isPaused;
        }
    }

    public void DisplayPauseMenu()
    {
        InventoryCanvas.SetActive(true);
        PartyStatsCanvas.SetActive(true);
    }

    public void HidePauseMenu()
    {
        InventoryCanvas.SetActive(false);
        PartyStatsCanvas.SetActive(false);
    }

}
