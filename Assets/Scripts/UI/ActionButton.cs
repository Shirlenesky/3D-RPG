using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;

    private SlotHolder currSlotHolder;

    private void Awake()
    {
        currSlotHolder = GetComponent<SlotHolder>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(actionKey) && currSlotHolder.itemUI.GetItem())
        {
            currSlotHolder.UseItem();
        }    
    }
}
