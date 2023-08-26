using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire//任务完成需要的内容数据结构
    {
        public string name;//需要打的怪或者收集物品的名字

        public int requireAmount;

        public int currAmount;
    }

    public string questName;

    [TextArea]
    public string questInfo;

    public bool isStarted;

    public bool isComplete;

    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();//任务需求列表

    public List<InventoryItem> rewards = new List<InventoryItem>();//奖励列表

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currAmount);

        isComplete = finishRequires.Count() == questRequires.Count();

        if(isComplete)
        {
            //Debug.Log("完成任务");
        }
    }

    //返回当前需要完成任务 消灭/收集 目标名称列表
    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();

        foreach (var require in questRequires)
        {
            targetNameList.Add(require.name);
        }

        return targetNameList;
    }

    public void GiveReward()
    {
        foreach (var reward in rewards)
        {
            if(reward.amount < 0)
            {
                int requireCount = Mathf.Abs(reward.amount);

                if(InventoryManager.Instance.QuestItemInBag(reward.itemData) != null)
                {
                    if(InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;

                        if(InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                        {
                            InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                        }
                    }
                    else
                    {
                        InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else
                {
                    InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }
            InventoryManager.Instance.inventoryUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
            
        }
    }
}
