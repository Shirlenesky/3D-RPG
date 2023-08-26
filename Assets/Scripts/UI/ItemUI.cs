using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;

    public Text amout = null;

    public ItemData_SO curritemData;

    public InventoryData_SO Bag { get; set; }

    public int Index { get; set; } = -1;


    public void SetUpItemUI(ItemData_SO itemdata, int itemamount)//π‹¿ÌImage ∫Õ Text
    {   
        if(itemamount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if(itemamount < 0)
        {
            itemdata = null;
        }

        if(itemdata != null)
        {
            curritemData = itemdata;
            icon.sprite = itemdata.itemIcon;
            amout.text = itemamount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }
}
