using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask//任务列表中的数据格式
    {
        public QuestData_SO questData;

        public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
        public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
        public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    public bool HaveQuest(QuestData_SO data)//判断上面那个tasks 是否有当前任务data，直接用名字判断
    {
        if (data != null)
        {

            return tasks.Any(q => q.questData.questName == data.questName);
        }
        else
        {

            return false;
        }
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }


    /// <summary>
    /// 敌人死亡，物品拾取时调用，但不一定符合task，所以要判断
    /// </summary>
    /// <param name="requireName"></param>
    /// <param name="amount"></param>
    public void UpdateQuestProgress(string requireName, int amount)
    {
        foreach (var task in tasks)
        {
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if(matchTask != null)
            {
                matchTask.currAmount += amount;
            }
            task.questData.CheckQuestProgress();
        }
    }
}
