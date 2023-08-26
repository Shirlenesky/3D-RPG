using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData, int amount)
    {
        bool isfound = false;

        //可堆叠物品 1. 背包中已有，直接修改数据 2. 背包中没有，找最近空格放入
        //不可堆叠 直接放入
        
        if(newItemData.stackable)
        {
           //1
            foreach (var item in items)
            {
                if(item.itemData == newItemData)
                {
                    item.amount += amount;
                    isfound = true;

                    break;
                }
            }
        }
        //2 3 
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null && !isfound)
            {
                items[i].itemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }

    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int amount;//实际背包中物品的数量
}
