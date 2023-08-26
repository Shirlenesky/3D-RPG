using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;

    public Text itemInfoText;

    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        UpdatePostion();
    }

    private void Update()
    {
        UpdatePostion();
    }

    public void UpdatePostion()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if(mousePos.y < height)
        {
            rect.position = mousePos + Vector3.up * height * 0.6f;
        }
        else if(Screen.width - mousePos.x > width)
        {
            rect.position = mousePos + Vector3.right * width * 0.6f;
        }
        else
        {
            rect.position = mousePos + Vector3.left * width * 0.6f;
        }
    }

    public void SetUpTooltip(ItemData_SO itemData)
    {
        itemNameText.text = itemData.itemName;
        itemInfoText.text = itemData.itemdescription;
    }

}
