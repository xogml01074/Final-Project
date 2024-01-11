using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager sfx;

    public AudioSource sfxSource;
    public Slider sfxSlider;

    private void Awake()
    {
        if (sfx == null)
        {
            sfx = this;

            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXSound");
    }
}
