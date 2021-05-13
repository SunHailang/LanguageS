using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ScrollViewDirection : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField]
    private RectTransform m_scrollview;
    [SerializeField]
    private RectTransform m_scrollviewContent;
    [SerializeField]
    private GameObject m_directionPrefab;


    private List<int> m_directionItems = new List<int>();
    private List<ScrollViewItem> m_items = new List<ScrollViewItem>();

    private static float m_centerX;
    private static float m_spacing = 30.0f;

    private void Start()
    {
        m_directionItems.Clear();

        m_centerX = m_scrollview.sizeDelta.x / 2 - 5.5f / 2;

        for (int i = 0; i < 360; i++)
        {
            m_directionItems.Add(i);
        }
        int count = Mathf.RoundToInt(m_scrollview.sizeDelta.x / m_spacing) + 1;
        if (count % 2 == 0) count++;
        for (int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(m_directionPrefab, m_scrollviewContent.transform);
            RectTransform rectItem = item.GetComponent<RectTransform>();
            float x = (5.5f / 2) + i * (5.5f + m_spacing);
            rectItem.localPosition = new Vector3(x, 0, 0);
            item.GetComponent<ScrollViewItem>().Configure(i, m_directionItems[i]);
            m_items.Add(item.GetComponent<ScrollViewItem>());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"OnDrag:: {eventData.delta}");

        //m_scrollviewContent.localPosition += new Vector3(eventData.delta.x, 0, 0);
        if (eventData.delta.x < 0)
        {
            // left move
        }
        if (eventData.delta.x > 0)
        {
            // right move
        }

        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].transform.localPosition += new Vector3(eventData.delta.x, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
    }

    public float m_perTargetAngle = 0.0f;
    public float m_targetAngle = 0.0f;

    private bool m_scrollState = false;

    int m_angleValue = 0;
    float m_angleDecimalValue = 0.0f;

    /// <summary>
    /// true : right
    /// false : left
    /// </summary>
    private bool m_angleStep = true;

    public void ResetDirection(float angle)
    {
        // 0 <= angle <= 359
        if (angle == m_targetAngle) return;
        m_targetAngle = angle;

        m_angleStep = GetAngleRed(m_targetAngle, m_perTargetAngle, out float angleValue);
        m_angleValue = Mathf.FloorToInt(m_targetAngle);
        m_angleDecimalValue = m_targetAngle - m_angleValue;
        m_scrollState = angleValue > 0;
        //Debug.Log($"m_targetAngle::{m_targetAngle}, m_perTargetAngle::{m_perTargetAngle}, BoolStep::{m_angleStep}");
        //Debug.Log($"angleValue Befor::{angleValue}, Total After::{m_total}");
    }

    private bool GetAngleRed(float target, float per, out float angleValue)
    {
        // t: 30, p: 350
        if (target > per)
        {
            float x1 = 360 - target + per;
            float x2 = target - per;
            angleValue = Mathf.Min(x1, x2);
            return x1 < x2;
        }
        else
        {
            float x1 = 360 - per + target;
            float x2 = per - target;
            angleValue = Mathf.Min(x1, x2);
            return x1 > x2;
        }
    }


    private void Update()
    {
        if (!m_scrollState)
        {
            m_perTargetAngle = m_targetAngle;
            return;
        }

        Transform transValue = m_scrollviewContent.transform.Find($"{m_angleValue}");
        float nextValue = -1f;
        if (transValue != null)
        {
            nextValue = Mathf.Abs(m_centerX - transValue.localPosition.x);
        }
        float deltaX = 0;
        if (nextValue < 0)
            deltaX = Mathf.Lerp(0, m_centerX, 0.026f);
        else
            deltaX = Mathf.Lerp(0, nextValue, 0.027f);

        nextValue = nextValue > 0 ? nextValue : deltaX;
        if (nextValue <= 2f)
        {
            deltaX = nextValue + m_angleDecimalValue * (m_spacing + 5.5f);
            m_scrollState = false;
        }
        deltaX *= (m_angleStep ? 1.0f : -1.0f);

        float posX = 100;
        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].transform.localPosition += new Vector3(deltaX, 0, 0);
            float pos = Mathf.Abs(m_items[i].transform.localPosition.x - m_centerX);
            if (pos < posX)
            {
                m_perTargetAngle = m_items[i].itemValue;
                posX = pos;
            }
        }
    }

    private void LateUpdate()
    {
        if (m_scrollviewContent == null || m_scrollviewContent.transform.childCount <= 0)
            return;

        RectTransform first = m_scrollviewContent.GetChild(0).GetComponent<RectTransform>();
        RectTransform end = m_scrollviewContent.GetChild(m_scrollviewContent.childCount - 1).GetComponent<RectTransform>();

        if (end.localPosition.x < m_scrollview.sizeDelta.x)
        {
            float x = end.localPosition.x + m_spacing + 5f;
            first.localPosition = new Vector3(x, 0, 0);
            ScrollViewItem firstItem = first.GetComponent<ScrollViewItem>();
            ScrollViewItem endItem = end.GetComponent<ScrollViewItem>();
            int index = endItem.indexValue + 1;
            index = index >= m_directionItems.Count ? 0 : index;
            firstItem.Configure(index, m_directionItems[index]);
            first.SetAsLastSibling();
            return;
        }

        if (first.localPosition.x > -2f)
        {
            float x = first.localPosition.x - m_spacing - 5f;
            end.localPosition = new Vector3(x, 0, 0);
            ScrollViewItem firstItem = first.GetComponent<ScrollViewItem>();
            ScrollViewItem endItem = end.GetComponent<ScrollViewItem>();
            int index = firstItem.indexValue - 1;
            index = index < 0 ? m_directionItems.Count - 1 : index;
            endItem.Configure(index, m_directionItems[index]);
            end.SetAsFirstSibling();
            return;
        }
    }


}
