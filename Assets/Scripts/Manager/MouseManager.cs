using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    RaycastHit hitInfo;
    public Texture2D point, doorway, attack, target, arrow;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(InteractWithUI() == false)
        {
            MouseControl();
        }
        SetCursorTexture();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("���߷���");
        if (Physics.Raycast(ray, out hitInfo))
        {
            //�л������ͼ
            //��ʵ��������Ϊʾ������δ��ȫʵ��
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
       
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if(hitInfo.collider.gameObject.tag == "Ground")
            {
                EventHandler.CallClickToMove(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                EventHandler.CallClickEnemyAttack(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.tag == "Attackable")
            {
                EventHandler.CallClickEnemyAttack(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.tag == "Portal")
            {
                EventHandler.CallClickToMove(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.tag == "Item")
            {
                EventHandler.CallClickToMove(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.tag == "NPC")
            {
                EventHandler.CallClickToMove(hitInfo.point);
            }
        }
    }

    private bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
