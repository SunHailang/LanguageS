using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashLoader : MonoBehaviour
{
    [SerializeField]
    private Transform m_video;
    [SerializeField]
    private RectTransform m_imageBG;

    private static readonly float m_staWidth = 1920.0f;
    private static readonly float m_staHeight = 1080.0f;

    private Vector3 m_scaleVec3 = Vector3.one;

    private void OnEnable()
    {
        float width = Screen.width / m_staWidth;
        float height = Screen.height / m_staHeight;

        m_scaleVec3 = new Vector3(1920f, 10f, 1080f) * Mathf.Max(width, height) / 10f;
        m_video.localScale = Vector3.zero;
        m_imageBG.sizeDelta = new Vector2(m_scaleVec3.x, m_scaleVec3.z) * 10f;
        m_imageBG.localScale = Vector3.one;
    }

    private void Start()
    {
        StartCoroutine(SetImageBG());
    }

    private IEnumerator SetImageBG()
    {
        float imageBGTimes = 1.5f;
        float imageTimes = 0.0f;
        while (m_imageBG.gameObject.activeSelf)
        {
            yield return null;
            imageTimes += Time.deltaTime;
            if (imageTimes >= imageBGTimes)
            {
                m_imageBG.localScale = Vector3.one * (imageTimes / -3.0f + 1.0f);
                m_imageBG.Rotate(Vector3.forward, imageTimes);
                if (m_imageBG.localScale.x <= 0.0f) m_imageBG.gameObject.SetActive(false);
            }
        }
        StartCoroutine(SetVideoBG());
    }

    private IEnumerator SetVideoBG()
    {
        float imageTimes = 0.0f;
        float scaleMin = Mathf.Min(m_scaleVec3.x, m_scaleVec3.z);
        Vector3 scale = m_scaleVec3 / scaleMin;
        while (imageTimes <= 3.0f)
        {
            yield return null;
            m_video.localScale = scale * (imageTimes / 3.0f) * scaleMin;
            float angle = (630 + 90) / 9.0f * Mathf.Pow(3.0f - imageTimes, 2) - 90f;
            m_video.rotation = Quaternion.Euler(new Vector3(angle, -90f, 90f));
            imageTimes += Time.deltaTime;
        }
        m_video.localScale = m_scaleVec3;
        m_video.rotation = Quaternion.Euler(new Vector3(630f, -90f, 90f));
    }

    public void BtnStart_OnClick()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

}
