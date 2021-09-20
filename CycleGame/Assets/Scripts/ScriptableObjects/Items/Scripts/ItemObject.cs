using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Heal,
    Equipment,
    Default
}

public enum Attributes
{
    Strength,
    Defense,
    Special,
    Heal
}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite UIDisplay;
    public ItemType Type;
    [TextArea(15,20)] //makes reading the desc easier in editor
    public string Description;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public ItemBuff[] buffs;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].Value);
            buffs[i].Attribute = item.buffs[i].Attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes Attribute;
    public int Value;

    public ItemBuff(int value)
    {
        Value = value;
    }
}