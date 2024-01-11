using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager ui;

    public GameObject onlineBtns;
    public GameObject options;

    private void Awake()
    {
        if (ui == null)
        {
            ui = this;

            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    public void OnClickOptionsSave()
    {
        PlayerPrefs.SetFloat("BGMSound", BGMManager.bgm.bgmSlider.value);
        PlayerPrefs.SetFloat("SFXSound", SFXManager.sfx.sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void OnClickOnline()
    {
        onlineBtns.SetActive(true);
    }

    public void OnClickOptions()
    {
        options.SetActive(true);
    }

    public void OnClickExitOptions()
    {
        options.SetActive(false);
    }
}
