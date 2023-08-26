using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Useable, Weapon, Armor}

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/ItemData")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;

    public string itemName;

    public Sprite itemIcon;

    public bool stackable;

    public int itemCount;//这个是掉落物的数量

    [TextArea]
    public string itemdescription = "";

    [Header("---- Useable ----")]
    public UseableItemData_SO useableData;

    [Header("---- Weapon ----")]

    public GameObject weaponPrefab;

    public AttackData_SO weaponData;

    public AnimatorOverrideController weaponAnimator;

}
