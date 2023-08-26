using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemUI currItemUI;

    private SlotHolder currHolder;

    private SlotHolder targetHolder;

    private void Awake()
    {
        currItemUI = GetComponent<ItemUI>();
        currHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("begin drag");
        InventoryManager.Instance.currDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currDrag.originSlotHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currDrag.originParent = (RectTransform)transform.parent;

        

        //��¼ԭʼ����
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //�������
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ��������
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckActionUI(eventData.position) || InventoryManager.Instance.CheckEquipmentyUI(eventData.position)|| InventoryManager.Instance.CheckInInventoryUI(eventData.position))
            {
                if(eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                if (targetHolder != InventoryManager.Instance.currDrag.originSlotHolder)
                {
                    //Debug.Log("Ŀ��" + targetHolder.name + " ԭʼ" + InventoryManager.Instance.currDrag.originSlotHolder.name);
                    switch (targetHolder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currItemUI.Bag.items[currItemUI.Index].itemData.itemType == ItemType.Weapon)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ARMOR:
                            if (currItemUI.Bag.items[currItemUI.Index].itemData.itemType == ItemType.Armor)
                            {
                                SwapItem();
                            }
                            break;
                        case SlotType.ACTION:
                            if (currItemUI.Bag.items[currItemUI.Index].itemData.itemType == ItemType.Useable)
                            {
                                SwapItem();
                            }
                            break;
                    }
                    currHolder.UpdateItem();
                    targetHolder.UpdateItem();
                }
            }

        }

        transform.SetParent(InventoryManager.Instance.currDrag.originParent);

        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tmpItem = currHolder.itemUI.Bag.items[currHolder.itemUI.Index];

        bool isSameItem = targetItem.itemData == tmpItem.itemData;

        if(isSameItem  && targetItem.itemData.stackable)
        {
            targetItem.amount += tmpItem.amount;
            tmpItem.itemData = null;
            tmpItem.amount = 0;
        }
        else
        {
            currHolder.itemUI.Bag.items[currHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tmpItem;
        }
    }
}
