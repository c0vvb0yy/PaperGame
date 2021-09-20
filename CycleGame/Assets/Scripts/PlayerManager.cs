using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InventoryObject Inventory;
    public static GameObject Player;
    
    public static int Level;
    public static int EXP;
    public static int HP;

    private static Vector3 savedPos;

    public static bool IsTalking = false;
    public static bool FreeToMove = true;
    public static bool IsFighting = false;
    public static bool IsPaused = false;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");    
    }

    public static void ForbidEverything()
    {
        IsTalking = false;
        FreeToMove = false;
        IsFighting = false;
        IsPaused = false;
    }

    public static void EnterDialogue()
    {
        ForbidEverything();
        IsTalking = true;
    }

    public static void ExitDialogue()
    {
        ForbidEverything();
        FreeToMove = true;
    }

    public static void EnterBattle()
    {
        ForbidEverything();
        IsFighting = true;
    }

    public static void ExitBattle()
    {
        ForbidEverything();
        LoadPos();
        FreeToMove = true;
    }

    public static void PauseGame()
    {
        ForbidEverything();
        IsPaused = true;
    }
    
    public static void ResumeGame()
    {
        ForbidEverything();
        FreeToMove = true;
    }

    public static void SavePos()
    {
        savedPos = Player.transform.position;
    }

    public static void LoadPos()
    {
        Player.transform.position = savedPos;
    }

}
