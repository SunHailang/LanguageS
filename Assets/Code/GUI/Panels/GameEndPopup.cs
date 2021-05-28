using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum OpenType
{
    GameOver,
    VolumeSetting,
}

public class GameEndPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject m_tabGameOver = null;
    [SerializeField]
    private GameObject m_tabVolume = null;
    [SerializeField]
    private GameObject m_contentGameOver = null;
    [SerializeField]
    private GameObject m_contentVolume = null;

    [Space]
    [SerializeField]
    private Slider m_sliderBG = null;
    [SerializeField]
    private Slider m_sliderVideo = null;


    private void OnEnable()
    {
        App.Instance.Pause = true;
    }

    private void OnDisable()
    {
        App.Instance.Pause = false;
    }

    private void Start()
    {
        m_sliderBG.value = VolumeSettingController.Instance.VolumeBG;
        m_sliderVideo.value = VolumeSettingController.Instance.VolumeVideo;

        m_sliderBG.onValueChanged.AddListener((value)=> VolumeSettingController.Instance.SetVolumeBG(value));
        m_sliderVideo.onValueChanged.AddListener((value) => VolumeSettingController.Instance.SetVolumeVideo(value));


        SetTabContent(OpenType.GameOver);
    }


    public void BtnTabGameOver_OnClick()
    {
        SetTabContent(OpenType.GameOver);
    }

    public void BtnTabVolume_OnClick()
    {
        SetTabContent(OpenType.VolumeSetting);
    }

    private void SetTabContent(OpenType type)
    {
        m_tabGameOver.SetActive(type == OpenType.GameOver);
        m_contentGameOver.SetActive(type == OpenType.GameOver);
        m_tabVolume.SetActive(type == OpenType.VolumeSetting);
        m_contentVolume.SetActive(type == OpenType.VolumeSetting);

    }

    public void BtnRebirth_OnClick()
    {
        PlayerController.Instance.Rebirth();
        UIController.Instance.Close(UILevel.PanelLevel);
    }

    public void BtnQuit_OnClick()
    {
        UIController.Instance.Close(UILevel.PanelLevel);
    }
}
