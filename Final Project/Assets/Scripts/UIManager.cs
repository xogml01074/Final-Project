using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager ui;

    public GameObject onlineBtns;
    public GameObject options;

    public InputField inputNickName;

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

    private void Start()
    {
        inputNickName.text = PlayerPrefs.GetString("NickName");
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

    public void OnClickCreate()
    {
        NetworkCallback.Nc.RunGame(GameMode.Host);
    }

    public void OnClickJoin()
    {
        NetworkCallback.Nc.RunGame(GameMode.Client);
    }

    public void OnClickOptions()
    {
        options.SetActive(true);
    }

    public void OnClickExitOptions()
    {
        options.SetActive(false);
    }

    public void SetNickName()
    {
        PlayerPrefs.SetString("NickName", inputNickName.text);
        PlayerPrefs.Save();
    }
}
