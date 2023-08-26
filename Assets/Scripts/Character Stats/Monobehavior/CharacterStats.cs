using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    public CharacerData_SO templateData;

    public CharacerData_SO characerData;

    public AttackData_SO attackData;//原始数据，使用的数据
    public AttackData_SO baseAttackData;//原始数据副本

    private RuntimeAnimatorController baseAnimator;

    [Header("---- Weapon ----")]
    public Transform weaponSlot;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if(templateData)
        {
            characerData = Instantiate(templateData);
        }
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read From Data_SO
    public int MaxHealth 
    { 
        get
        {
            if(characerData != null)
                return characerData.maxHealth;
            else
                return 0;
        }
        set
        {
            characerData.maxHealth = value;
        }
    }

    public int CurrHealth
    {
        get
        {
            if (characerData != null)
                return characerData.currHealth;
            else
                return 0;
        }
        set
        {
            characerData.currHealth = value;
        }
    }

    public int BaseDefence
    {
        get
        {
            if (characerData != null)
                return characerData.baseDefence;
            else
                return 0;
        }
        set
        {
            characerData.baseDefence = value;
        }
    }

    public int CurrDefence
    {
        get
        {
            if (characerData != null)
                return characerData.currDefence;
            else
                return 0;
        }
        set
        {
            characerData.currDefence = value;
        }
    }

    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker, CharacterStats defencer)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defencer.CurrDefence, 0);
        CurrHealth = Mathf.Max(0, CurrHealth - damage);
        if (attacker.isCritical)
        {
            defencer.GetComponent<Animator>().SetTrigger("Hit");
        }
        UpdateHealthBarOnAttack?.Invoke(CurrHealth, MaxHealth); 
        if(CurrHealth <= 0)
        {
            attacker.characerData.UpdateExp(characerData.killPoint);
        }
    }

    public void TakeDamage(int damage, CharacterStats defencer)
    {
        int currDamage = Mathf.Max(damage - defencer.CurrDefence, 0);
        CurrHealth = Mathf.Max(0, CurrHealth - currDamage);
        //EventHandler.CallUpdateHealthBarOnAttack(CurrHealth, MaxHealth);
        UpdateHealthBarOnAttack?.Invoke(CurrHealth, MaxHealth);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if(isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int)coreDamage;
    }
    #endregion

    #region Equip Weapon
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }


    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab == null) return;
        Instantiate(weapon.weaponPrefab, weaponSlot);

        attackData.ApplyWeaponData(weapon.weaponData);
        //InventoryManager.Instance.UpdateStatsText(MaxHealth, attackData.minDamage, attackData.maxDamage);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }

    public void UnEquipWeapon()
    {
        if(weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }

        attackData.ApplyWeaponData(baseAttackData);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
        
    }
    #endregion

    #region Apply Data Change
    public void ApplyHealth(int amount)
    {
        if(CurrHealth + amount <= MaxHealth)
        {
            CurrHealth += amount;
        }
        else
        {
            CurrHealth = MaxHealth;
        }
    }
    #endregion
}
