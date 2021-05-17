using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class SliderValueAnimation : MonoBehaviour
{
    private Slider m_slider;

    private float m_targetValue;


    private void Awake()
    {
        m_slider = GetComponent<Slider>();
    }

    public void SetTargetValue(float value)
    {
        m_targetValue = value;
    }

    private void Update()
    {
        float v = Mathf.Abs(m_targetValue - m_slider.value);
        float a = Mathf.Pow(v, 1.8f) + (v < 0.02f ? 0.02f - v : v > 0.03 ? v : 0.02f) + 0.005f;
        if (v < 0.005f) a = 1.0f;
        m_slider.value = Mathf.Lerp(m_slider.value, m_targetValue, a);
    }
}
