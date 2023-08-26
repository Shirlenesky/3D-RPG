using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public static class EventHandler
{
    public static event Action<Vector3> ClickToMove;
    public static void CallClickToMove(Vector3 pos)
    {
        ClickToMove?.Invoke(pos);
    }

    public static event Action<GameObject> ClickEnemyAttack;
    public static void CallClickEnemyAttack(GameObject enemy)
    {
        ClickEnemyAttack?.Invoke(enemy);
    }

    //想不到好办法
    public static event Action<int, int> UpdateHealthBarOnAttack;
    public static void CallUpdateHealthBarOnAttack(int currHealth, int maxHealth)
    {
        UpdateHealthBarOnAttack?.Invoke(currHealth, maxHealth);
    }
}
