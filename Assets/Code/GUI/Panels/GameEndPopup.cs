using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndPopup : MonoBehaviour
{
   
    public void BtnRebirth_OnClick()
    {
        PlayerController.Instance.Rebirth();
        UIController.Instance.Close(UILevel.PanelLevel);
    }

    public void BtnQuit_OnClick()
    {
        UIController.Instance.Close(UILevel.PanelLevel);
    }
}
