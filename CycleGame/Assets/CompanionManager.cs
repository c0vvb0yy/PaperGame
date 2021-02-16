using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    private static Vector3 savedPos;

    public static bool IsFighting;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void EnterBattle()
    {
        IsFighting = true;
    }

    public static void ExitBattle()
    {
        IsFighting = false;
    }

    public void SavePos()
    {
        savedPos = transform.position;
    }

    public void LoadPos()
    {
        if(savedPos != null)
            transform.position = savedPos;
    }
}
