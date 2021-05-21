﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private InputMoveController m_inputMoveController;
    [SerializeField]
    private InputDirectionController m_inputDirectionController;

    [SerializeField]
    private ScrollViewDirection m_scrollviewDir;


    [SerializeField]
    private TextMeshProUGUI m_directionText;
    [SerializeField]
    private Transform m_allowDirection;
    [SerializeField]
    private Button m_btnJump;
    [SerializeField]
    private TextMeshProUGUI m_pickE;
    [SerializeField]
    private Button m_btnE;
    [SerializeField]
    private Button m_btnSetting;

    private Vector3 m_perMoveDirection = Vector3.forward;
    private Vector3 playerMoveDirection = Vector3.zero;

    private bool m_isDeath = false;

    private void Awake()
    {
        // regist event
        EventManager<Events>.Instance.RegisterEvent(Events.PickType, OnPickShow);
        EventManager<Events>.Instance.RegisterEvent(Events.PlayerLifeState, OnPlayerLifeState);

        m_inputMoveController.playerMoveController += SetPlayerMove;
        m_inputDirectionController.playerDirectionController += SetPlayerDirection;

        m_btnJump.onClick.AddListener(BtnJump_OnClick);
        m_btnE.onClick.AddListener(BtnE_OnClick);
        m_btnSetting.onClick.AddListener(() =>
        {
            UIController.Instance.Open<GameEndPopup>("GameEndPopup", UILevel.PanelLevel);
        });
    }

    private void OnDestroy()
    {
        // deregist event
        EventManager<Events>.Instance.DeregisterEvent(Events.PickType, OnPickShow);
        EventManager<Events>.Instance.DeregisterEvent(Events.PlayerLifeState, OnPlayerLifeState);
    }

    private Collider[] m_colliders = null;
    private void OnPickShow(Events arg1, object[] arg2)
    {
        if (arg2 != null && arg2.Length > 0)
        {
            m_colliders = arg2 as Collider[];
            m_pickE.text = "E";
            m_pickE.gameObject.SetActive(true);
            m_btnE.interactable = true;
            m_btnE.image.sprite = m_btnE.spriteState.highlightedSprite;
        }
        else
        {
            m_colliders = null;
            m_pickE.gameObject.SetActive(false);
            m_btnE.interactable = false;
            m_btnE.image.sprite = m_btnE.spriteState.disabledSprite;
        }
    }

    private void OnPlayerLifeState(Events arg1, object[] arg2)
    {
        m_isDeath = !Convert.ToBoolean(arg2[0]);
        if (m_isDeath)
        {
            // Show Death UI
            UIController.Instance.Open<GameEndPopup>("GameEndPopup", UILevel.PanelLevel);
        }
    }

    private void SetPlayerMove(Vector3 move)
    {
        move.z = playerMoveDirection.z;
        PlayerController.Instance.SetPlayerMove(move);
    }

    private void SetPlayerDirection(Vector2 direction)
    {
        Vector3 playerDirection = PlayerController.Instance.SetPlayerDirection(direction);
        SetDirection(playerDirection.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) UIController.Instance.Open<QuitGame>("QuitGame", UILevel.PanelLevel);
        if (Input.GetKeyDown(KeyCode.E) && m_colliders != null) PickE();

        playerMoveDirection.z = 0.0f;
        if (Input.GetButtonDown("Jump")) JumpSpace();
    }

    private void JumpSpace()
    {
        if (IsDeath() || PlayerData.Instance.playerMagic < 0.1f)
            playerMoveDirection.z = 0.0f;
        else
            playerMoveDirection.z = 1.0f;
        PlayerController.Instance.SetPlayerMove(playerMoveDirection);
    }

    private void BtnE_OnClick()
    {
        if (IsDeath()) return;
        if (m_colliders != null)
        {
            PickE();
        }
    }

    private void PickE()
    {
        if (IsDeath()) return;
        ActionEvent[] actions = new ActionEvent[m_colliders.Length];
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i] = m_colliders[i].GetComponent<ActionEvent>();
        }
        EventManager<Events>.Instance.TriggerEvent(Events.HurtType, actions);
        for (int i = 0; i < m_colliders.Length; i++)
        {
            Destroy(m_colliders[i].gameObject);
        }
    }

    private void BtnJump_OnClick()
    {
        JumpSpace();
    }


    private void SetDirection(float angleY)
    {
        float angle = angleY < 0f ? angleY + 360f : angleY > 360f ? angleY - 360f : angleY;
        string angleStr = "";
        if (angle > (360.0f - 22.5f) || angle <= 22.5f)
            angleStr = "E";
        else if (angle > 22.5f && angle <= (22.5f + 45.0f))
            angleStr = "ES";
        else if (angle > (22.5f + 45.0f) && angle <= (22.5f + 90.0f))
            angleStr = "S";
        else if (angle > (22.5f + 90.0f) && angle <= (180.0f - 22.5f))
            angleStr = "WS";
        else if (angle > (180.0f - 22.5f) && angle <= (180.0f + 22.5f))
            angleStr = "W";
        else if (angle > (180.0f + 22.5f) && angle <= (270.0f - 22.5f))
            angleStr = "WN";
        else if (angle > (270.0f - 22.5f) && angle <= (270.0f + 22.5f))
            angleStr = "N";
        else if (angle > (270.0f + 22.5f) && angle <= (360.0f - 22.5f))
            angleStr = "EN";

        m_directionText.text = string.Format("{0}:{1:F1}°", angleStr, angle);
        m_allowDirection.rotation = Quaternion.Euler(0, 0, angle - 90f);
        m_scrollviewDir.ResetDirection(angle);
    }


    private bool IsDeath()
    {
        if (m_isDeath)
        {
            playerMoveDirection = Vector3.zero;
            return true;
        }
        return false;
    }
}