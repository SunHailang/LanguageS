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
    private Camera m_videoCamera;

    private static readonly float m_staWidth = 1920.0f;
    private static readonly float m_staHeight = 1080.0f;

    private Vector3 m_scaleVec3 = new Vector3(1920f, 10f, 1080f);

    public float m_X = 17.75f;

    private void OnEnable()
    {

    }

    private void Update()
    {
        //float fov = m_videoCamera.fieldOfView;
        //float dis = Vector3.Distance(m_video.position, m_videoCamera.transform.position);
        //float si = Mathf.Sin(fov * Mathf.Deg2Rad);
        //float height = si * Mathf.Abs(dis) * 2;

        float width = Screen.width;
        float height = Screen.height;

        float scale = height * m_staWidth / m_staHeight;

        m_video.localScale = m_scaleVec3 * (width / height) / m_X;
    }

    public void BtnStart_OnClick()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

}
