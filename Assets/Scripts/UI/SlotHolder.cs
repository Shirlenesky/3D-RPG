using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG, WEAPON, ARMOR, ACTION}

public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount % 2 ==0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() == null) return;

        if(itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
        {
            GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);

            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if(itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerStats.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerStats.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
         if(itemUI.GetItem())
        {
            InventoryManager.Instance.itemTooltip.SetUpTooltip(itemUI.GetItem());
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false) ;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
