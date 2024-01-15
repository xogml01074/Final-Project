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
<<<<<<< HEAD
=======
    
    public GameObject lobby;
    public GameObject loding;
    public Button refereshBtn;
    public Transform sessionListContent;
    public GameObject sessionPrefab;
>>>>>>> 166c8dd6b818daeb139fb63dfd01ee0377b178b3

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
<<<<<<< HEAD
        onlineBtns.SetActive(true);
=======
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
>>>>>>> 166c8dd6b818daeb139fb63dfd01ee0377b178b3
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
