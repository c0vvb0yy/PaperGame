using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    public InventoryObject Inventory;
    public InventoryObject EquippedItems;

    public Dictionary<InventorySlot, bool> _EquippedItems = new Dictionary<InventorySlot, bool>();
    private BattleUnit characterStats;
    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<BattleUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyEquipmentStats()
    {
        for (int i = 0; i < EquippedItems.Container.Items.Length; i++)
        {
            var item = EquippedItems.Container.Items[i].Item.buffs;
            if (item == null) continue;
            foreach (var buff in item)
            {
                switch (buff.Attribute)
                {
                    case (Attributes.Strength):
                    {
                        characterStats.AddAttack(buff.Value);
                        break;
                    }
                    case (Attributes.Defense):
                    {
                        characterStats.AddDefense(buff.Value);
                        break;
                    }
                    default:
                    {
                        Debug.Log("Not an Equipment Object");
                        break;
                    }
                }
            }
        }
    }

    public void RemoveEquipmentStats()
    {
        
    }
    
}
