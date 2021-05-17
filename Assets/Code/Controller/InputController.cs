using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private ScrollViewDirection m_scrollviewDir;

    [SerializeField]
    private RectTransform m_inputKnob;
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

    private const float m_maxSpeed = 125.0f;
    private bool m_startDrag = false;

    private bool m_isDeath = false;

    private void Awake()
    {
        // regist event
        EventManager<Events>.Instance.RegisterEvent(Events.PickType, OnPickShow);
        EventManager<Events>.Instance.RegisterEvent(Events.PlayerLifeState, OnPlayerLifeState);

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

    private void OnEnable()
    {
        m_inputKnob.anchoredPosition = Vector3.zero;
    }

    private void Start()
    {
        SetDirection(m_perMoveDirection);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_startDrag = true;
        SetPosition(eventData, m_inputKnob);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_startDrag = false;
        m_inputKnob.localPosition = Vector3.zero;

        playerMoveDirection = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_startDrag = true;
        SetPosition(eventData, m_inputKnob);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData, m_inputKnob);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_startDrag = false;
        m_inputKnob.localPosition = Vector3.zero;

        playerMoveDirection = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) UIController.Instance.Open<QuitGame>("QuitGame", UILevel.PanelLevel);
        if (Input.GetKeyDown(KeyCode.E) && m_colliders != null) PickE();

        if (!m_startDrag && !IsDeath())
            playerMoveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), playerMoveDirection.z);

        if (Input.GetButtonDown("Jump")) JumpSpace();

        if (IsDeath()) playerMoveDirection = Vector3.zero;
        if (playerMoveDirection.x != 0.0f || playerMoveDirection.y != 0.0f)
        {
            m_perMoveDirection = playerMoveDirection;
            SetDirection(m_perMoveDirection);
        }
        PlayerController.Instance.SetPlayerMoveDirection(playerMoveDirection);
        playerMoveDirection.z = 0.0f;
    }

    private void JumpSpace()
    {
        if (IsDeath() || PlayerData.Instance.playerMagic < 0.1f)
        {
            return;
        }
        playerMoveDirection.z = 1.0f;
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

    private void SetPosition(PointerEventData eventData, RectTransform rect)
    {
        if (IsDeath()) return;
        //存储当前鼠标所在位置
        Vector3 rectWorldVec;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out rectWorldVec))
        {
            rect.position = rectWorldVec;

            float curDis = Vector3.Distance(Vector3.zero, rect.localPosition);
            if (curDis > m_maxSpeed)
            {
                //指定原点和方向
                Vector3 direction = rect.localPosition - Vector3.zero;
                Ray ray = new Ray(Vector3.zero, direction.normalized);
                Vector3 targetVec = ray.GetPoint(m_maxSpeed);
                rect.localPosition = targetVec;
            }

            float distance = Vector3.Distance(rect.localPosition, Vector3.zero);
            float value = Mathf.Max(0, distance / m_maxSpeed);
            playerMoveDirection = rect.localPosition.normalized * value;
        }
    }

    private void SetDirection(Vector3 direction)
    {
        direction = direction != Vector3.zero ? direction.normalized : Vector3.forward;
        // Set Direction

        float angleLeftValue = Vector3.Dot(direction, Vector3.left);
        float angleLeft = Mathf.Acos(angleLeftValue) * Mathf.Rad2Deg;

        float angleDownValue = Vector3.Dot(direction, Vector3.down);
        float angleDown = Mathf.Acos(angleDownValue) * Mathf.Rad2Deg;
        float angle = 0;

        if (angleLeft >= 0 && angleLeft < 90)
        {
            if (angleDown >= 0 && angleDown < 90)
                angle = (90.0f - angleLeft) + 270.0f;
            else
                angle = angleLeft;
        }
        else
        {
            if (angleDown >= 0 && angleDown < 90)
                angle = (90.0f - angleDown) + 180.0f;
            else
                angle = angleLeft;
        }
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
            m_inputKnob.localPosition = Vector3.zero;
            playerMoveDirection = Vector3.zero;
            return true;
        }
        return false;
    }
}
