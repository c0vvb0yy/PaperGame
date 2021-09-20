using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu (fileName ="New Inventory", menuName = "ScriptableObjects/InventorySystem")]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    public string SavePath;
    public ItemDatabase database;
    public Inventory Container;

   /* private void OnEnable() 
    {
#if UNITY_EDITOR
        database = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/Items/Database.asset", typeof(ItemDatabase));
#else
        database = Resources.Load<ItemDatabaseObject>("Prefabs/Items/Database.asset");
#endif
    }*/

    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if(Container.Items[i].Item.Id == item.Id)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        }
        
        Container.Items.Add(new InventorySlot(item, item.Id, amount));

    }

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        bf.Serialize(file, saveData);
        file.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, SavePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

   /* public void OnAfterDeserialize()
    {
        for(int i = 0; i < Container.Items.Count; i++)
        {
            Container.Items[i].Item = database.GetItem[Container.Items[i].ID]; 
        }
    }

   public void OnBeforeSerialize()
   {
   }*/
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}

[System.Serializable]
public class InventorySlot
{
    public Item Item;
    public int ID;
    public int Amount;
    

    public InventorySlot(Item item, int id, int amount)
    {
        Item = item;
        ID = id;
        Amount = amount;
    }

    public void AddAmount(int value)
    {
        Amount += value;
    }
}