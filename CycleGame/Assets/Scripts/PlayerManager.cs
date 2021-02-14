using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject Player;

    private static Vector3 savedPos;

    public static bool IsTalking = false;
    public static bool FreeToMove = true;


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");    
    }

    public void EnterDialogue()
    {
        IsTalking = true;
        FreeToMove = false;
    }

    public void ExitDialogue()
    {
        IsTalking = false;
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
