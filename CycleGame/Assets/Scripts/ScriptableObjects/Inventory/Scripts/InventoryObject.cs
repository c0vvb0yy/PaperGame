using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu (fileName ="New Inventory", menuName = "ScriptableObjects/InventorySystem")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string SavePath;
    private ItemDatabase database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable() 
    {
#if UNITY_EDITOR
        database = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/Items/Database.asset", typeof(ItemDatabase));
#else
        database = Resources.Load<ItemDatabaseObject>("Prefabs/Items/Database.asset");
#endif
    }

    public void AddItem(ItemObject item, int amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if(Container[i].Item == item)
            {
                Container[i].AddAmount(amount);
                return;
            }
        }
        
        Container.Add(new InventorySlot(item, database.GetId[item], amount));

    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

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

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < Container.Count; i++)
        {
            Container[i].Item = database.GetItem[Container[i].ID]; 
        }
    }

   public void OnBeforeSerialize()
   {
   }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject Item;
    public int ID;
    public int Amount;
    

    public InventorySlot(ItemObject item, int id, int amount)
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