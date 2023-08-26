using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("---- Elements ----")]
    public GameObject questPanel;

    public ItemTooltip tooltip;

    bool isOpen;

    [Header("---- Quest Name ----")]//˼·�������ǣ������ؼ���λ�� �����崴����prefab
    public RectTransform questListTransform;

    public QuestNameButton questNameButton;

    [Header("---- Quest Content ----")]
    public Text questContentText;

    [Header("---- Requirement ----")]
    public RectTransform requireTransform;

    public QuestRequirement requirement;

    [Header("---- Reward Panel ----")]
    public RectTransform rewardTransform;

    public ItemUI rewardUI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = "";
            //��ʾ�������
            SetupQuestList();

            if(!isOpen )
            {
                tooltip.gameObject.SetActive(false); 
            }
        }

    }

    public void SetupQuestList()
    {
        foreach (Transform item in questListTransform)//������ఴť
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)//���ٽ���������
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)//x��������
        {
            Destroy(item.gameObject);
        }

        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);

            newTask.SetupNameButton(task.questData);
            newTask.questContentText = questContentText;//������Questnamebtn �б��������ݵ�
        }
    }

    public void SetupRequireList(QuestData_SO data)//�������ݵ�����
    {
        foreach (Transform item in requireTransform)//x��������
        {
            Destroy(item.gameObject);
        }
        foreach (var require in data.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);

            if(data.isFinished)
            {
                q.SetupRequirement(require.name, data.isFinished);
            }

            q.SetupRequirement(require.name, require.requireAmount, require.currAmount);
        }

    }

    public void SetupRewardItem(ItemData_SO itemData, int amout)//����������� ��Ʒ���� ������
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetUpItemUI(itemData, amout);
    }

}
