using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;

    private Button thisButton;

    private DialogPiece currDialogPiece;

    private string nextPieceID;

    private bool takeQuest;


    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClick);
    }

    //更新option中的内容，那一条Piece的option
    public void UpdateOption(DialogPiece piece, DialogOption option)
    {
        currDialogPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;
    }

    public void OnOptionClick()
    {
        if(currDialogPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currDialogPiece.quest)
            };
            if(takeQuest)
            {
                //接取任务，添加到任务栏
                //先判断是否已经存在
                if(QuestManager.Instance.HaveQuest(newTask.questData))//存在
                {
                    //是否完成
                    if(QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveReward();
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //没有任务，接受新任务
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;

                    foreach (var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if(nextPieceID == "")
        {
            DialogUI.Instance.dialogPanel.SetActive(false);
        }
        else
        {
            DialogUI.Instance.UpdateMainDialog(DialogUI.Instance.currentData.dialogIndex[nextPieceID]);
        }
    }
}
