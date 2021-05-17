using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void BtnQuit_OnClick()
    {
        Application.Quit();
    }

    public void BtnCancel_OnClick()
    {
        UIController.Instance.Close(UILevel.PanelLevel);
    }
}
