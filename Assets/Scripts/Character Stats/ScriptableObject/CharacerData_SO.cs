using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Charater Stats/Data")]
public class CharacerData_SO : ScriptableObject
{
    [Header("---- Stats Info ----")]
    public int maxHealth;

    public int currHealth;

    public int baseDefence;

    public int currDefence;

    [Header("---- Kill ----")]
    public int killPoint;

    [Header("---- Level ----")]
    public int currentLevel;

    public int maxLevel;

    public int baseExp;

    public int currtExp;

    public float levelBuff;

    public float levelMultiplier { get { return 1 + (currentLevel - 1) * levelBuff; } }

    public void UpdateExp(int point)
    {
        currtExp += point;
        if(currtExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel+1, 0, maxLevel);
        baseExp += (int)(baseExp * levelMultiplier);

        maxHealth = (int)(maxHealth * levelMultiplier);
        currHealth = maxHealth;

    }
}
