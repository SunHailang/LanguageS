using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class SliderValueAnimation : MonoBehaviour
{
    private Slider m_slider;

    private float m_targetValue;

    private float m_difference;


    private void Awake()
    {
        if (m_slider == null)
            m_slider = GetComponent<Slider>();
    }

    public void SetMaxAndMinValue(float max, float min)
    {
        if (m_slider == null) m_slider = GetComponent<Slider>();
        m_slider.maxValue = max;
        m_slider.minValue = min;
    }

    public void SetTargetValue(float value)
    {
        m_targetValue = value;
        m_difference = m_targetValue - m_slider.value;
    }

    private void Update()
    {
        float value = m_slider.value + (m_difference / 0.45f) * Time.unscaledDeltaTime;
        m_slider.value = value > m_slider.maxValue ? m_slider.maxValue : value < m_slider.minValue ? m_slider.minValue : value;
    }
}
