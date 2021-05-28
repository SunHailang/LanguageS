using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoItemController : MonoBehaviour
{
    private AudioSource[] m_videoAudios;

    private void Awake()
    {
        m_videoAudios = GetComponentsInChildren<AudioSource>();
        SetAudioVolume();

        EventManager<VolumeEvents>.Instance.RegisterEvent(VolumeEvents.VolumeVideo, OnVolumeVideo);
    }

    private void OnVolumeVideo(VolumeEvents arg1, object[] arg2)
    {
        SetAudioVolume();
    }

    private void SetAudioVolume()
    {
        if (m_videoAudios == null) return;
        foreach (var item in m_videoAudios)
        {
            item.volume = VolumeSettingController.Instance.VolumeVideo;
        }
    }

    private void OnDestroy()
    {
        EventManager<VolumeEvents>.Instance.DeregisterEvent(VolumeEvents.VolumeVideo, OnVolumeVideo);
    }
}
