using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Animation", menuName = "ScriptableObjects/Animation", order = 1)]
public class Animation : ScriptableObject
{
    
    public KeyFrame[] KeyFrames;
    
    [Serializable]
    public struct KeyFrame
    {
        
        public float Duration;
        public Sprite Sprite;

       /* public Sprite GetSprite()
        {
            return Sprite;
        }
        */
    }
}
