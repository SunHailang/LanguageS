using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewLoopController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private List<int> m_itemDatas = new List<int>();
    [SerializeField]
    private ScrollRect m_scrollview = null;
    [SerializeField]
    private RectTransform m_scrollviewViewport = null;
    [SerializeField]
    private Transform m_scrollviewContent = null;

    [SerializeField]
    private RectTransform m_prefab = null;

    private List<ScrollViewItem> m_items = new List<ScrollViewItem>();

    private bool m_beginDrag = false;
    private bool m_endDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        m_beginDrag = true;
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
        m_endDrag = true;
    }

    private void Awake()
    {
        for (int i = 1; i <= 50; i++)
        {
            m_itemDatas.Add(i);
        }
        int index = 1000 / 120;
        for (int i = 0; i < index + 1; i++)
        {
            RectTransform itemRect = Instantiate(m_prefab, m_scrollviewContent);
            ScrollViewItem item = itemRect.GetComponent<ScrollViewItem>();
            float x = 60 + i * (120 + 50);
            itemRect.localPosition = new Vector3(x, 0, 0);
            item.Configure(i, m_itemDatas[i]);
            m_items.Add(item);
        }
    }

    private void LateUpdate()
    {
        RectTransform first = m_scrollviewContent.GetChild(0).GetComponent<RectTransform>();
        RectTransform end = m_scrollviewContent.GetChild(8).GetComponent<RectTransform>();

        if (end.localPosition.x < 1060f)
        {
            float x = end.localPosition.x + 50f + 120f;
            first.localPosition = new Vector3(x, 0, 0);
            ScrollViewItem firstItem = first.GetComponent<ScrollViewItem>();
            ScrollViewItem endItem = end.GetComponent<ScrollViewItem>();
            int index = endItem.indexValue + 1;
            index = index >= m_itemDatas.Count ? index - m_itemDatas.Count : index;
            firstItem.Configure(index, m_itemDatas[index]);
            first.SetAsLastSibling();
            return;
        }

        if (first.localPosition.x > -60f)
        {
            float x = first.localPosition.x - 50f - 120f;
            end.localPosition = new Vector3(x, 0, 0);
            ScrollViewItem firstItem = first.GetComponent<ScrollViewItem>();
            ScrollViewItem endItem = end.GetComponent<ScrollViewItem>();
            int index = firstItem.indexValue - 1;
            index = index < 0 ? m_itemDatas.Count - 1 : index;            
            endItem.Configure(index, m_itemDatas[index]);
            end.SetAsFirstSibling();
            return;
        }
    }

}
