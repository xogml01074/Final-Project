using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // ���� ���� ���
    public enum GameState
    {
        Start,
        GameOver,
        Clear,
    }

    // ������ ���� ����
    public GameState gState { get; set; }

    // ���� ���� UI ������Ʈ ����
    public Text gameTxt;

    private void Start()
    {
        gState = GameState.Start;
    }

    private void Update()
    {
        ClearOrNot();
    }

    private void ClearOrNot()
    {
        if (gState == GameState.Clear)
        {
            gameTxt.text = "Ŭ����!";

            Time.timeScale = 0f;
            StartCoroutine(GoLobby());
            return;
        }

        if (gState == GameState.GameOver)
        {
            gameTxt.text = "�������";

            Time.timeScale = 0f;
            StartCoroutine(GoLobby());
            return;
        }
    }

    IEnumerator GoLobby()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(0);
    }
}
