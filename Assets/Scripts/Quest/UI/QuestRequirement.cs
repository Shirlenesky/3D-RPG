using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireNameText;//���

    private Text progressText;//�Ҳ����

    private void Awake()
    {
        requireNameText = GetComponent<Text>();
        progressText = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name, int amount, int currAmount)
    {
        requireNameText.text = name;
        progressText.text = currAmount.ToString() + " / " + amount.ToString();
    }

    public void SetupRequirement(string name, bool isFinished)
    {
        if(isFinished)
        {
            requireNameText.text = name;
            progressText.text = "���";
            requireNameText.color = Color.gray;
            progressText.color = Color.gray;
        }
    }
}
