using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputMoveController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform m_inputKnob;


    public event System.Action<Vector3> playerMoveController;

    private const float m_maxSpeed = 125.0f;

    private Vector3 m_perMoveDirection = Vector3.forward;
    private Vector3 playerMoveDirection = Vector3.zero;

    private bool m_startDrag = false;

    private bool m_isDeath = false;

    private void OnEnable()
    {
        m_inputKnob.anchoredPosition = Vector3.zero;
    }

    private IEnumerator Start()
    {
        yield return null;
        playerMoveController?.Invoke(m_perMoveDirection);
    }

    private void Update()
    {
        if (!IsDeath())
        {
            playerMoveDirection.z = 0.0f;
            if (Input.GetButtonDown("Jump")) JumpSpace();

            if (!m_startDrag)
            {
                playerMoveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), playerMoveDirection.z);
            }
            playerMoveController?.Invoke(playerMoveDirection);
        }
    }
    public void JumpSpace()
    {
        if (IsDeath() || PlayerData.Instance.playerMagic < 0.1f)
            playerMoveDirection.z = 0.0f;
        else
            playerMoveDirection.z = 1.0f;
        //PlayerController.Instance.SetPlayerMove(playerMoveDirection);
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

        playerMoveController?.Invoke(Vector3.zero);
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

        playerMoveController?.Invoke(Vector3.zero);
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
            playerMoveController?.Invoke(rect.localPosition.normalized * value);
        }
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
