using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // ���� ���� ���
    public enum GameState
    {
        Start,
        GameOver,
        Clear,
    }

    // ������ ���� ����
    public GameState gState = GameState.Start;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }


    // ���� ���� UI ������Ʈ ����
    public Text gameTxt;

    private void Update()
    {
        ClearOrNot();
    }

    public void ClearOrNot()
    {
        if (gState == GameState.Clear)
        {
            gameTxt.text = "Ŭ����!";

            StartCoroutine(GoLobby());
            Time.timeScale = 0f;
            return;
         }

        if (gState == GameState.GameOver)
        {
            gameTxt.text = "�������";

            StartCoroutine(GoLobby());
            Time.timeScale = 0f;
            return;
        }
    }

    IEnumerator GoLobby()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }
}
