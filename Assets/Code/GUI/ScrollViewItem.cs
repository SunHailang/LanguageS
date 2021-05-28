using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollViewItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text = null;

    [HideInInspector]
    public int indexValue = 0;
    [HideInInspector]
    public int itemValue = 0;

    public void Configure(int index, int value)
    {
        indexValue = index;
        itemValue = value;
        gameObject.name = $"{value}";
        if (value % 5 == 0)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(5.5f, 110.0f);
            if (value % 15 == 0)
            {
                m_text.text = $"{value}°";
                m_text.gameObject.SetActive(true);
            }
            else
            {
                m_text.gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(5.5f, 80.0f);
            m_text.gameObject.SetActive(false);
        }
    }
}
