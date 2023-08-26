using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogPiece
{
    public string ID;

    public Sprite image;

    [TextArea]
    public string text;

    public QuestData_SO quest;

    public List<DialogOption> options = new List<DialogOption>();
}
