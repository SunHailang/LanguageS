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

    private void Start()
    {
        m_directionItems.Clear();

        m_centerX = m_scrollview.sizeDelta.x / 2 - 5.5f / 2;

        for (int i = 0; i < 360; i++)
        {
            m_directionItems.Add(i);
        }
        int count = Mathf.RoundToInt(m_scrollview.sizeDelta.x / 50) + 1;
        if (count % 2 == 0) count++;
        for (int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(m_directionPrefab, m_scrollviewContent.transform);
            RectTransform rectItem = item.GetComponent<RectTransform>();
            float x = (5.5f / 2) + i * (5.5f + 50);
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

    private float m_perTargetAngle = 0.0f;
    private float m_targetAngle = 0.0f;

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

        float targetRad = Mathf.Deg2Rad * m_targetAngle;
        float perRad = Mathf.Deg2Rad * m_perTargetAngle;

        m_angleStep = GetAngleRed(m_targetAngle, m_perTargetAngle);

        m_total = m_total * (50 + 5.5f);
    }

    private bool GetAngleRed(float target, float per)
    {
        if (target > per)
        {
            float x1 = 360 - target + per;
            float x2 = target - per;
            m_total = Mathf.Min(x1, x2);
            return x1 < x2;
        }
        else
        {
            float x1 = 360 - per + target;
            float x2 = per - target;
            m_total = Mathf.Min(x1, x2);
            return x1 > x2;
        }


        //int value1 = GetAnglePos(target);
        //int value2 = GetAnglePos(per);

        //int v1 = (value2 + 1) > 4 ? 1 : (value2 + 1);
        //int v2 = (v1 + 1) > 4 ? 1 : (v1 + 1);

        //if (value2 == value1 || value2 == v1)
        //{
        //    m_total = Mathf.Abs(target - per);
        //    return target - per > 0;
        //}
        //else if (value2 == v2)
        //{
        //    bool ret = (per - target) > (target + 360 - per);
        //    if (ret) m_total = per - target;
        //    else m_total = target + 360 - per;
        //    return ret;
        //}
        //else
        //{
        //    m_total = target + 360 - per;
        //    return false;
        //}
    }

    private int GetAnglePos(float angle)
    {
        if (angle >= 0.0f && angle < 90.0f) return 1;
        else if (angle >= 90.0f && angle < 180.0f) return 2;
        else if (angle >= 180.0f && angle < 270.0f) return 3;
        else return 4;
    }

    float m_step = 500f;
    float m_total = 0.0f;
    private void Update()
    {
        if (m_total <= 0)
        {
            m_perTargetAngle = m_targetAngle;
            return;
        }

        float deltaX = Time.deltaTime * m_step * (m_angleStep ? 1.0f : -1.0f);
        if (deltaX >= m_total)
        {
            deltaX = m_total;
        }

        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].transform.localPosition += new Vector3(deltaX, 0, 0);
        }
        if (m_angleStep)
            m_total -= deltaX;
        else
            m_total += deltaX;
    }

    private void LateUpdate()
    {
        if (m_scrollviewContent == null || m_scrollviewContent.transform.childCount <= 0)
            return;

        RectTransform first = m_scrollviewContent.GetChild(0).GetComponent<RectTransform>();
        RectTransform end = m_scrollviewContent.GetChild(m_scrollviewContent.childCount - 1).GetComponent<RectTransform>();

        if (end.localPosition.x < m_scrollview.sizeDelta.x)
        {
            float x = end.localPosition.x + 50f + 5f;
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
            float x = first.localPosition.x - 50f - 5f;
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
