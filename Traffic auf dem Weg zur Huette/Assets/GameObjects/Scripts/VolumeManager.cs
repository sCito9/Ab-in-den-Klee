using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        source.volume = MusicManager.Instance.value;
        MusicManager.Instance.OnMusicToggle += HandleVolumeChange;
    }

    private void HandleVolumeChange(float value)
    {
        source.volume = value;
    }
}
