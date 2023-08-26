using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUI : Singleton<DialogUI>
{
    [Header("---- Basic Element ----")]
    public Image icon;

    public Text mainText;

    public Button nextButton;

    public GameObject dialogPanel;

    [Header("---- Options ----")]
    public RectTransform optionPanel;

    public OptionUI optionPrefab;//option btn

    [Header("---- Data ----")]
    public DialogData_SO currentData;

    int currIndex = 0;


    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialog);
    }

    private void ContinueDialog()
    {
        if(currIndex < currentData.dialogPieces.Count)
        {
            UpdateMainDialog(currentData.dialogPieces[currIndex]);
        }
        else
        {
            dialogPanel.SetActive(false);
        }
    }

    public void UpdateDialogData(DialogData_SO data)//把对话数据从Dialog Controller拿过来
    {
        currentData = data;
        currIndex = 0;
    }

    public void UpdateMainDialog(DialogPiece dialogPiece)//更新dialogPanel中内容
    {
        currIndex++;
        dialogPanel.SetActive(true);
        if(dialogPiece.image != null)//是否有对话头像需要显示
        {
            icon.enabled = true;
            icon.sprite = dialogPiece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = "";
        //mainText.text = dialogPiece.text;
        mainText.DOText(dialogPiece.text, 1f);

        if(dialogPiece.options.Count == 0 && currentData.dialogPieces.Count > 0)//没有选择项 且 还有后续对话，next键显示
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            //nextButton.gameObject.SetActive(false) ;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        CreateOptions(dialogPiece);
        //options
    }

    void CreateOptions(DialogPiece piece)
    {
        if(optionPanel.childCount > 0)//上一段对话中有多个选项
        {
            //先销毁
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        //再生成
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);

            option.UpdateOption(piece, piece.options[i]);
        }
    }
}
