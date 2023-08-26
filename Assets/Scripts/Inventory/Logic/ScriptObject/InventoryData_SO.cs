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

        //�ɶѵ���Ʒ 1. ���������У�ֱ���޸����� 2. ������û�У�������ո����
        //���ɶѵ� ֱ�ӷ���
        
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

    public int amount;//ʵ�ʱ�������Ʒ������
}
