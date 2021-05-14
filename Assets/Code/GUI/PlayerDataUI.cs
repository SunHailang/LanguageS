using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField]
    private SliderValueAnimation m_sliderBlood;
    [SerializeField]
    private SliderValueAnimation m_sliderMagic;

    private float m_targetBloodValue;
    private float m_targetMagicValue;

    private void Awake()
    {
        // register event
        EventManager<Events>.Instance.RegisterEvent(Events.HurtType, OnHurtType);
    }

    private void Update()
    {
        m_sliderBlood.SetTargetValue(PlayerData.Instance.playerBlood);
        m_sliderMagic.SetTargetValue(PlayerData.Instance.playerMagic);
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
                        PlayerData.Instance.SetPlayerData(pick.replyType, pick.Value / 100);
                        break;
                    case SkillActionType.ExplosionType:
                        PlayerData.Instance.SetPlayerData(ReplyType.Blood, action[i].Value / 100);
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
