using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "ScriptableObjects/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake() 
    {
        Type = ItemType.Default;    
    }
}
