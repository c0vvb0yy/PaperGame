using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool IsTalking = false;
    public static bool FreeToMove = true;

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

}
