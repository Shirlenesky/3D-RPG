using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//button本身显示出来的任务名称

    public QuestData_SO currData;//点击button 传输到右边显示的data

    public Text questContentText;//右边显示的content

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentText.text = currData.questInfo;//任务详情文本给到

        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        QuestUI.Instance.SetupRequireList(currData);//奖励面板给到
        foreach (var item in currData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemData, item.amount);
        }
    }

    public void SetupNameButton(QuestData_SO questData)
    {
        currData = questData;

        if(questData.isComplete)
        {
            questNameText.text = questData.questName + "(已完成)";
        }
        else
        {
            questNameText.text = questData.questName;
        }
    }
}
