using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal,
    Equipment,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject Prefab;
    public ItemType Type;
    [TextArea(15,20)] //makes reading the desc easier in editor
    public string Description;



}
