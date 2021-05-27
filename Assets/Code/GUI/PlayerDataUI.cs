using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField]
    private SliderValueAnimation m_sliderBlood;
    [SerializeField]
    private SliderValueAnimation m_sliderMagic;
    [Space]
    [SerializeField]
    private TextMeshProUGUI m_score;


    private void Awake()
    {
        this.m_sliderBlood.SetMaxAndMinValue(PlayerData.Instance.playerMaxBlood, 0.0f);
        this.m_sliderMagic.SetMaxAndMinValue(PlayerData.Instance.playerMaxMagic, 0.0f);
        // register event
        EventManager<Events>.Instance.RegisterEvent(Events.HurtType, OnHurtType);
    }

    private void Update()
    {
        this.m_sliderBlood.SetTargetValue(PlayerData.Instance.playerBlood);
        this.m_sliderMagic.SetTargetValue(PlayerData.Instance.playerMagic);

        this.m_score.text = string.Format("Score: {0}", PlayerData.Instance.playerSorce);
    }

    private void OnHurtType(Events arg1, object[] arg2)
    {
        if (arg2 != null && arg2.Length > 0)
        {
            ActionEvent[] action = arg2 as ActionEvent[];
            for (int i = 0; i < action.Length; i++)
            {
                switch (action[i].actionType)
                {
                    case SkillActionType.PickType:
                        PickActionEvent pick = action[i] as PickActionEvent;
                        PlayerData.Instance.SetPlayerData(pick.replyType, pick.Value);
                        break;
                    case SkillActionType.ExplosionType:
                        PlayerData.Instance.SetPlayerData(ReplyType.Blood, action[i].Value);
                        break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        // deregister event
        EventManager<Events>.Instance.DeregisterEvent(Events.HurtType, OnHurtType);
    }
}
