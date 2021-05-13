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
    private Button m_jumpButton;
    [SerializeField]
    private TextMeshProUGUI m_pickE;

    private Vector3 m_perMoveDirection = Vector3.forward;
    private Vector3 playerMoveDirection = Vector3.zero;

    private const float m_maxSpeed = 125.0f;
    private bool m_startDrag = false;

    private void Awake()
    {
        // regist event
        EventManager<Events>.Instance.RegisterEvent(Events.PickType, OnPickShow);
        m_jumpButton.onClick.AddListener(BtnJump_OnClick);
    }
    private void OnDestroy()
    {
        // deregist event
        EventManager<Events>.Instance.DeregisterEvent(Events.PickType, OnPickShow);
    }
    private Collider[] m_colliders = null;
    private void OnPickShow(Events arg1, object[] arg2)
    {
        if (arg2 != null && arg2.Length > 0)
        {
            m_colliders = arg2 as Collider[];
            m_pickE.text = "E";
            m_pickE.gameObject.SetActive(true);
        }
        else
        {
            m_colliders = null;
            m_pickE.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.E) && m_colliders != null)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                Destroy(m_colliders[i].gameObject);
            }
        }

        if (!m_startDrag)
        {
            Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), playerMoveDirection.z);
            playerMoveDirection = moveDir;
        }

        if (Input.GetButtonDown("Jump"))
        {
            playerMoveDirection.z = 1.0f;
        }

        if (playerMoveDirection.x != 0.0f || playerMoveDirection.y != 0.0f)
        {
            m_perMoveDirection = playerMoveDirection;
            SetDirection(m_perMoveDirection);
        }
        PlayerController.Instance.SetPlayerMoveDirection(playerMoveDirection);
        playerMoveDirection.z = 0.0f;
    }

    private void BtnJump_OnClick()
    {
        Debug.Log("BtnJump_OnClick");
        playerMoveDirection.z = 1.0f;
    }

    private void SetPosition(PointerEventData eventData, RectTransform rect)
    {
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

        m_scrollviewDir.ResetDirection(angle);
    }
}
