using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public event Action<float> OnMusicToggle;

    public float value = 0.5f;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void toggled()
    {
        var slider = FindAnyObjectByType<Slider>();
        OnMusicToggle?.Invoke(slider.value); 
        value = slider.value;
    }
}
