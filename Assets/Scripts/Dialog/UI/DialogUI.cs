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

    public void UpdateDialogData(DialogData_SO data)//�ѶԻ����ݴ�Dialog Controller�ù���
    {
        currentData = data;
        currIndex = 0;
    }

    public void UpdateMainDialog(DialogPiece dialogPiece)//����dialogPanel������
    {
        currIndex++;
        dialogPanel.SetActive(true);
        if(dialogPiece.image != null)//�Ƿ��жԻ�ͷ����Ҫ��ʾ
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

        if(dialogPiece.options.Count == 0 && currentData.dialogPieces.Count > 0)//û��ѡ���� �� ���к����Ի���next����ʾ
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
        if(optionPanel.childCount > 0)//��һ�ζԻ����ж��ѡ��
        {
            //������
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        //������
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);

            option.UpdateOption(piece, piece.options[i]);
        }
    }
}
