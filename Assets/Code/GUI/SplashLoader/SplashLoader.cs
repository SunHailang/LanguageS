using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SplashLoader : MonoBehaviour
{
    [SerializeField]
    private Transform m_video = null;
    [SerializeField]
    private RectTransform m_imageBG = null;
    [SerializeField]
    private GameObject m_btnStart = null;

    private Material m_imageMaterial;

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
        m_btnStart.transform.localScale = Vector3.zero;
        m_btnStart.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SetImageBG());

        m_imageMaterial = m_imageBG.GetComponent<Image>().material;
        Vector3[] points = new Vector3[3];
        points[0] = Vector3.zero;
        points[1] = new Vector3(0.2f, 0.2f);
        points[2] = Vector3.one;
        vecs = BezierUtils.GetBezierPoints(points);
    }
    Vector3[] vecs;
    float m_tTime = 0.0f;
    bool start = true;
    private void Update()
    {
        if (m_tTime <= 1.0f && start)
        {
            int index = Mathf.RoundToInt((vecs.Length - 1) * m_tTime);
            m_imageBG.localScale = vecs[index];
            m_tTime += Time.deltaTime;
            if (m_tTime > 1.0f) start = false;
        }

        if (m_tTime <= 1.0f && m_btnStart.activeSelf)
        {
            int index = Mathf.RoundToInt((vecs.Length - 1) * m_tTime);
            m_btnStart.transform.localScale = vecs[index];
            m_tTime += Time.deltaTime;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Input.mousePosition;

        //    m_imageMaterial.SetVector("_mousePos", new Vector4(mousePos.x / Screen.width, mousePos.y / Screen.height, mousePos.z, 1));

        //    m_imageMaterial.SetFloat("_mousePosX", mousePos.x / Screen.width);
        //    m_imageMaterial.SetFloat("_mousePosY", mousePos.y / Screen.height);

        //    Debug.Log($"x:{mousePos.x / Screen.width}, y:{mousePos.y / Screen.height}");
        //}
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

        m_tTime = 0.0f;
        m_btnStart.SetActive(true);
    }

    public void BtnStart_OnClick()
    {
        SceneManager.LoadSceneAsync("SampleScene");

        //start = true;
        //m_tTime = 0.0f;
    }

}
