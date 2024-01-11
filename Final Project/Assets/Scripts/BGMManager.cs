using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public static BGMManager bgm;

    public AudioSource bgmSource;
    public Slider bgmSlider;

    private void Awake()
    {
        if (bgm == null)
        {
            bgm = this;

            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMSound");
    }
}
