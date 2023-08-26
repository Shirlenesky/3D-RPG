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

    [Header("---- Quest Name ----")]//思路基本就是，创建控件的位置 ，具体创建的prefab
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
            //显示面板内容
            SetupQuestList();

            if(!isOpen )
            {
                tooltip.gameObject.SetActive(false); 
            }
        }

    }

    public void SetupQuestList()
    {
        foreach (Transform item in questListTransform)//销毁左侧按钮
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)//销毁奖励栏内容
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)//x销毁需求
        {
            Destroy(item.gameObject);
        }

        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);

            newTask.SetupNameButton(task.questData);
            newTask.questContentText = questContentText;//这里是Questnamebtn 中变量传数据的
        }
    }

    public void SetupRequireList(QuestData_SO data)//需求内容的生成
    {
        foreach (Transform item in requireTransform)//x销毁需求
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

    public void SetupRewardItem(ItemData_SO itemData, int amout)//奖励面板生成 物品数据 ，数量
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetUpItemUI(itemData, amout);
    }

}
