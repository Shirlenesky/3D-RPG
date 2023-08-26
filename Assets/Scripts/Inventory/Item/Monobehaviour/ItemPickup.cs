using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;//每个可拾取物品自身的数据

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemCount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备武器
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //检查是否有任务
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemCount);
            //删除自己
            Destroy(gameObject);
        }
    }
}
