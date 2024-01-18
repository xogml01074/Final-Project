using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager ui;

    public GameObject menu;
    public GameObject options;

    public GameObject lobby;
    public GameObject loding;
    public Button refereshBtn;
    public Transform sessionListContent;
    public GameObject sessionPrefab;

    public int characterChoice = 0;

    public InputField inputNickName;

    private void Awake()
    {
        if (ui == null)
        {
            ui = this;

            DontDestroyOnLoad(gameObject);
        }
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

        StartCoroutine(Loding());
    }

    IEnumerator Loding()
    {
        yield return new WaitForSeconds(0.2f);
        Text lodingTxt = loding.GetComponentInChildren<Text>();
        lodingTxt.text = "Loding...";
        loding.SetActive(true);

        yield return new WaitForSeconds(5);
        loding.SetActive(false);
        NetworkCallback.Nc.RefreshSessionListUI();
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

    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToCC()
    {
        SceneManager.LoadScene("CharacterChoiceScene");
    }
}
