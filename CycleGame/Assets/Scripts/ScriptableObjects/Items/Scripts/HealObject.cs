using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Object", menuName = "ScriptableObjects/Items/Heal")]
public class HealObject : ItemObject
{
    public int RestoreHealthValue;
    public void Awake() 
    {
        Type = ItemType.Heal;    
    }
}

