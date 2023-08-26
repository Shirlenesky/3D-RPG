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

    //����option�е����ݣ���һ��Piece��option
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
                //��ȡ������ӵ�������
                //���ж��Ƿ��Ѿ�����
                if(QuestManager.Instance.HaveQuest(newTask.questData))//����
                {
                    //�Ƿ����
                    if(QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveReward();
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //û�����񣬽���������
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
