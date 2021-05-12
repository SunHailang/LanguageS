using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private ScrollViewDirection m_scrollviewDir;

    [SerializeField]
    private RectTransform m_inputKnob;
    [SerializeField]
    private TextMeshProUGUI m_directionText;

    private Vector3 m_perMoveDirection = Vector3.forward;
    private Vector3 playerMoveDirection = Vector3.zero;

    private const float m_maxSpeed = 125.0f;

    private void OnEnable()
    {
        m_inputKnob.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetPosition(eventData, m_inputKnob);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_inputKnob.localPosition = Vector3.zero;

        playerMoveDirection = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetPosition(eventData, m_inputKnob);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData, m_inputKnob);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_inputKnob.localPosition = Vector3.zero;

        playerMoveDirection = Vector3.zero;
    }

    private void Update()
    {
        if (playerMoveDirection != Vector3.zero)
        {
            m_perMoveDirection = playerMoveDirection;
            SetDirection(m_perMoveDirection);
        }
        PlayerController.Instance.SetPlayerMoveDirection(playerMoveDirection);
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
