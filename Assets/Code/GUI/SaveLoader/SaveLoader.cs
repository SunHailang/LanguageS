using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoader : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_imageBG = null;
    [SerializeField]
    private Slider m_slider = null;
    [SerializeField]
    private TextMeshProUGUI m_sliderText = null;

    private static readonly float m_staWidth = 1920.0f;
    private static readonly float m_staHeight = 1080.0f;
    private void Awake()
    {
        m_slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }
    void Start()
    {
        if (m_imageBG != null)
        {
            float width = Screen.width / m_staWidth;
            float height = Screen.height / m_staHeight;
            Vector3 m_scaleVec3 = new Vector3(1920f, 10f, 1080f) * Mathf.Max(width, height) / 10f;
            m_imageBG.sizeDelta = new Vector2(m_scaleVec3.x, m_scaleVec3.z) * 10f;
            m_imageBG.localScale = Vector3.one;
        }        
    }

    private void Slider_OnValueChanged(float value)
    {
        value *= 100;
        m_sliderText.text = string.Format("{0:F1}%", value);
    }

}
