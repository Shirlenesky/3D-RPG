using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;//button������ʾ��������������

    public QuestData_SO currData;//���button ���䵽�ұ���ʾ��data

    public Text questContentText;//�ұ���ʾ��content

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentText.text = currData.questInfo;//���������ı�����

        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        QuestUI.Instance.SetupRequireList(currData);//����������
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
            questNameText.text = questData.questName + "(�����)";
        }
        else
        {
            questNameText.text = questData.questName;
        }
    }
}
