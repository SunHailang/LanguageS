using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SaveLoader : MonoBehaviour
{
    [SerializeField]
    private Transform m_planeBG = null;
    [SerializeField]
    private Slider m_slider = null;
    [SerializeField]
    private TextMeshProUGUI m_sliderText = null;

    public VideoClip videoClip;

    private static readonly float m_staWidth = 1920.0f;
    private static readonly float m_staHeight = 1080.0f;

    private bool m_update = false;
    private SceneData m_nextScene = null;

    private void Awake()
    {
        m_slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }

    void Start()
    {
        m_slider.value = 1.0f;

        if (m_planeBG != null)
        {
            float width = Screen.width / m_staWidth;
            float height = Screen.height / m_staHeight;
            Vector3 m_scaleVec3 = new Vector3(1.0f, 10f, m_staHeight / m_staWidth) * Mathf.Max(width, height);
            m_planeBG.localScale = m_scaleVec3 / 2.8f;
        }

        m_nextScene = SceneController.Instance.GetNextScene();

        StartCoroutine(LoadVideo());
    }

    private IEnumerator LoadVideo()
    {
        VideoPlayer video = m_planeBG.GetComponent<VideoPlayer>();

        video.playOnAwake = false;
        video.clip = videoClip;
        video.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        video.targetMaterialRenderer = m_planeBG.GetComponent<Renderer>();
        video.targetMaterialProperty = "_MainTex";
        video.audioOutputMode = VideoAudioOutputMode.None;

        if (!video.isPlaying)
            video.Play();

        yield return null;
    }

    private IEnumerator LoadNextScene()
    {

        if (m_nextScene == null)
        {
            Debug.LogError("Load Next Scene Error.");
            yield break;
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(m_nextScene.nextName, LoadSceneMode.Additive);
        while (!async.isDone)
        {
            m_slider.value = async.progress;
            yield return null;
        }

        while (m_slider.value < 1.0f)
        {
            yield return null;
            m_slider.value += Time.unscaledDeltaTime * 0.2f;
        }

        SceneManager.UnloadSceneAsync("SaveLoader");

        App.Instance.Pause = false;
    }

    private void Update()
    {
        if (!m_update && m_nextScene != null && m_nextScene.startNext)
        {
            m_update = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private void Slider_OnValueChanged(float value)
    {
        value *= 100;
        m_sliderText.text = string.Format("{0:F1}%", value);
    }

}
