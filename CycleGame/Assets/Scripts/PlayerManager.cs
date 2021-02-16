using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject Player;
    
    public static int Level;
    public static int EXP;

    private static Vector3 savedPos;

    public static bool IsTalking = false;
    public static bool FreeToMove = true;
    public static bool IsFighting = false;

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
        FreeToMove = true;
    }

    public static void SavePos()
    {
        savedPos = Player.transform.position;
    }

    public static void LoadPos()
    {
        if(savedPos != null)
            Player.transform.position = savedPos;
    }

}
