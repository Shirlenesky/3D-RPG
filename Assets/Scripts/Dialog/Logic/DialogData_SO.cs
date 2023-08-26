using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog Data")]
public class DialogData_SO : ScriptableObject
{
    public List<DialogPiece> dialogPieces = new List<DialogPiece>();

    public Dictionary<string, DialogPiece> dialogIndex = new Dictionary<string, DialogPiece>();


    public QuestData_SO GetQuest()
    {
        QuestData_SO currQuest = null;
        foreach (var piece in dialogPieces)
        {
            if(piece.quest != null)
            {
                currQuest = piece.quest;
            }
        }
        return currQuest;
    }
}
