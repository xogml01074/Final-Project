using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager ui;

    public GameObject menu;
    public GameObject options;
    private bool optionsActive;

    public GameObject bgm;
    public GameObject sfx;
    
    public GameObject lobby;
    public GameObject loding;
    public Button refereshBtn;
    public Transform sessionListContent;
    public GameObject sessionPrefab;

    public InputField inputNickName;

    private void Awake()
    {
        if (ui == null)
        {
            ui = this;

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(options);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inputNickName.text = PlayerPrefs.GetString("NickName");
    }

    private void Update()
    {
        OnClickESC();
    }

    public void OnClickESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!optionsActive)
            {
                BGMManager.bgm.bgmSlider.value = PlayerPrefs.GetFloat("BGMSound");
                SFXManager.sfx.sfxSlider.value = PlayerPrefs.GetFloat("SFXSound");

                options.SetActive(true);
                optionsActive = true;
                bgm.SetActive(true);
                sfx.SetActive(true);
            }

            else
            {
                bgm.SetActive(false);
                sfx.SetActive(false);
                options.SetActive(false);
                optionsActive = false;
            }
        }
    }

    public void OnClickOptionsSave()
    {
        PlayerPrefs.SetFloat("BGMSound", BGMManager.bgm.bgmSlider.value);
        PlayerPrefs.SetFloat("SFXSound", SFXManager.sfx.sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void OnClickOnline()
    {
        NetworkCallback.Nc.ConnectToLobby(inputNickName.text);
        menu.SetActive(false);
        lobby.SetActive(true);

        StartCoroutine(Loding());
    }
    
    IEnumerator Loding()
    {
        yield return new WaitForSeconds(0.2f);
        Text lodingTxt = loding.GetComponentInChildren<Text>();
        lodingTxt.text = "Loding...";
        loding.SetActive(true);

        yield return new WaitForSeconds(3);
        loding.SetActive(false);
    }

    public void OnClickCreate()
    {
        lobby.SetActive(false);
        NetworkCallback.Nc.CreateSession();
    }

    public void OnClickRefresh()
    {
        StartCoroutine(RefreshWait());
    }

    IEnumerator RefreshWait()
    {
        refereshBtn.interactable = false;
        NetworkCallback.Nc.RefreshSessionListUI();
        yield return new WaitForSeconds(3);
        refereshBtn.interactable = true;
    }

    public void OnClickOptions()
    {
        BGMManager.bgm.bgmSlider.value = PlayerPrefs.GetFloat("BGMSound");
        SFXManager.sfx.sfxSlider.value = PlayerPrefs.GetFloat("SFXSound");

        options.SetActive(true);
        bgm.SetActive(true);
        sfx.SetActive(true);
    }

    public void OnClickExitOptions()
    {
        bgm.SetActive(false);
        sfx.SetActive(false);
        options.SetActive(false);
    }

    public void SetNickName()
    {
        PlayerPrefs.SetString("NickName", inputNickName.text);
        PlayerPrefs.Save();
    }
}
