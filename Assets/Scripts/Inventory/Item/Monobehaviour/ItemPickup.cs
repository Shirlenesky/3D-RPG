using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;//ÿ����ʰȡ��Ʒ���������

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemCount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //װ������
            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //����Ƿ�������
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemCount);
            //ɾ���Լ�
            Destroy(gameObject);
        }
    }
}
