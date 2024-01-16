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
    
    public GameObject lobby;
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
        NetworkCallback.Nc.ConnectToLobby(inputNickName.text);
        menu.SetActive(false);
        lobby.SetActive(true);
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD

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
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
=======
>>>>>>> parent of 392b659 (Attacker멀티플레이 테스트)
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
