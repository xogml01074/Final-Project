using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // 게임 상태 상수
    public enum GameState
    {
        Start,
        GameOver,
        Clear,
    }

    // 현재의 게임 상태
    public GameState gState = GameState.Start;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }


    // 게임 상태 UI 오브젝트 변수
    public Text gameTxt;

    private void Update()
    {
        ClearOrNot();
    }

    public void ClearOrNot()
    {
        if (gState == GameState.Clear)
        {
            gameTxt.text = "클리어!";

            StartCoroutine(GoLobby());
            Time.timeScale = 0f;
            return;
         }

        if (gState == GameState.GameOver)
        {
            gameTxt.text = "전원사망";

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
