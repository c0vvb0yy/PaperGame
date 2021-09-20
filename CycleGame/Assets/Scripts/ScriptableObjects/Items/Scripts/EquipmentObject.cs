using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "ScriptableObjects/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    //public float AtkBonus;
    //public float DefBonus;
    
    public void Awake() 
    {
        Type = ItemType.Equipment;    
    }
}