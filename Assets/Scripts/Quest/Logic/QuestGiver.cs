using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogController))]
public class QuestGiver : MonoBehaviour
{
    DialogController dialogController;

    QuestData_SO currQuest;

    public DialogData_SO startDialog;
    public DialogData_SO progressDialog;
    public DialogData_SO completeDialog;
    public DialogData_SO finishedDialog;

    #region 获得任务状态
    public bool IsStarted
    {
        get
        {
            if(QuestManager.Instance.HaveQuest(currQuest))
            {
                return QuestManager.Instance.GetTask(currQuest).IsStarted;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currQuest))
            {
                return QuestManager.Instance.GetTask(currQuest).IsComplete;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currQuest))
            {
                return QuestManager.Instance.GetTask(currQuest).IsFinished;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion
    private void Awake()
    {
        dialogController = GetComponent<DialogController>();
    }

    private void Start()
    {
        dialogController.currDialogData = startDialog;
        currQuest = dialogController.currDialogData.GetQuest();
    }

    private void Update()
    {
        if(IsStarted)
        {
            if(IsComplete)
            {
                dialogController.currDialogData = completeDialog;
            }
            else
            {
                dialogController.currDialogData = progressDialog;
            }
        }

        if(IsFinished)
        {
            dialogController.currDialogData = finishedDialog;
        }
    }

}
