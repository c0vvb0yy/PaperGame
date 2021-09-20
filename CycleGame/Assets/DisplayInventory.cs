using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject Inventory;
    
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
        for (int i = 0; i < Inventory.Container.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(Inventory.Container[i]))
            {
                itemsDisplayed[Inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = Inventory.Container[i].Amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(Inventory.Container[i].Item.Prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = Inventory.Container[i].Amount.ToString("n0");
                itemsDisplayed.Add(Inventory.Container[i], obj);
            }
        }
    }
}
