using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    public static GameObject Companion;
    private static Vector3 savedPos;

    public static int HP;
    
    public static bool IsFighting;
    public static bool IsTalking;
    public static bool FreeToMove = true;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        Companion = GameObject.FindWithTag("Anthra");
    }

    // Update is called once per frame

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
        IsFighting = true;
    }

    public static void ExitBattle()
    {
        IsFighting = false;
    }

    public static void SavePos()
    {
        savedPos = Companion.transform.position;
    }

    public static void LoadPos()
    {
        Companion.transform.position = savedPos;
    }
}
