using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject Inventory;
    public GameObject inventoryPrefab;
    public int XStart;
    public int YStart;
    public int XBorderBetweenItems;
    public int NumberOfColumns;
    public int YBorderBetweenItems;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void OnEnable() 
    {
       UpdateDisplay();
    }

    public Vector3 GetPosition(int i) 
    {
        return new Vector3(XStart + (XBorderBetweenItems * (i % NumberOfColumns)), YStart + (-YBorderBetweenItems * (i / NumberOfColumns)), 0f);  
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = Inventory.Container.Items[i];

            if(itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory.database.GetItem[slot.Item.Id].UIDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
                itemsDisplayed.Add(slot, obj);
            }
        }
    }
}
