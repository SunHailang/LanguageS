using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollViewItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text;

    [HideInInspector]
    public int indexValue = 0;

    public void Configure(int index, int value)
    {
        indexValue = index;
        gameObject.name = $"{value}";
        if (value % 45 == 0)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(5.5f, 110.0f);
            m_text.text = $"{value}°";
            m_text.gameObject.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(5.5f, 80.0f);
            m_text.gameObject.SetActive(false);
        }
    }
}
