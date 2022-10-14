using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Singleton;

public class B_Inventory : MonoSingleton<B_Inventory>
{
    List<B_InventoryItem> itemList=new List<B_InventoryItem>();
    private int gold;

    public delegate void GoldChanged();
    public GoldChanged goldChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        var children = transform.GetComponentsInChildren<B_InventoryItem>().ToList();
        foreach (var child in children)
        {
            itemList.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) SetItemQuantity(200005, 20);
        if (Input.GetKeyDown(KeyCode.G)) SetGold(2000);
    }

    public int GetGold()
    {
        return gold;
    }
    
    public void SetGold(int val)
    {
        gold = val;
        goldChanged?.Invoke();
    }

    public void SubtractGold(int val)
    {
        gold -= val;
        goldChanged?.Invoke();
    }

    public B_InventoryItem GetItem(int itemID)
    {
        foreach (var item in itemList)
        {
            if(item.itemData.ID==itemID) return item;
        }
        return null;
    }
    
    public void SetItemQuantity(int itemID, int val)
    {
        foreach (var item in itemList)
        {
            if (item.itemData.ID == itemID)
            {
                item.SetQuantity(val);
            }
        }
    }

    public int GetItemQuantity(int itemID)
    {
        foreach (var item in itemList)
        {
            if(item.itemData.ID==itemID) return item.itemData.quantity;
        }
        return 0;
    }
}
