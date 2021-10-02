using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject[] PauseCanvasses;
    
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
        foreach (var canvas in PauseCanvasses)
        {
            canvas.SetActive(true);
        }
    }

    public void HidePauseMenu()
    {
        foreach (var canvas in PauseCanvasses)
        {
            canvas.SetActive(false);
        }
    }

}
