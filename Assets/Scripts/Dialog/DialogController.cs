using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogData_SO currDialogData;

    public bool canTalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && currDialogData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currDialogData != null)
        {
            DialogUI.Instance.dialogPanel.SetActive(false);
            canTalk = false;
        }
    }

    private void Update()
    {
        if(canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialog();
        }
    }

    public void OpenDialog()
    {
        //打开UI
        //传入对话
        DialogUI.Instance.UpdateDialogData(currDialogData);
        DialogUI.Instance.UpdateMainDialog(currDialogData.dialogPieces[0] );
    }
}
