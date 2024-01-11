using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Start,
        GameOver,
        Clear,
    }

    // 현재의 게임 상태 변수
    [Networked] public GameState gState { get; set; }

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // PlayerMove 클래스 변수
    public PlayerMove player;

    // PlayerController 클래스 변수
    public PlayerController player_Attacker;
    public List<GameObject> players;

    public PlayerRotate pr;

    public Text loginTime;
    public override void Spawned()
    {
        // 초기 게임 상태는 준비 상태로 설정한다.
        gState = GameState.Ready;

        // 게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());
    }

    public override void FixedUpdateNetwork()
    {
        ClearOrNot();
    }

    IEnumerator ReadyToStart()
    {
        // 2초간 대기한다.
        yield return new WaitForSeconds(1f);

        // 상태를 "게임 중" 상태로 변경한다.
        gState = GameState.Start;
    }

    private void ClearOrNot()
    {
        if (gState == GameState.Clear)
        {
            gameLabel.SetActive(true);
            gameText.text = "클리어!";

            Time.timeScale = 0f;
        }

        else if (gState == GameState.GameOver)
        {
            gameLabel.SetActive(true);
            gameText.text = "전원사망";

            Time.timeScale = 0f;
        }
    }

    // 게임 종료 옵션
    public void QuitGame()
    {
        // 애플리케이션을 종료한다.
        Application.Quit();
    }

    public void AddPlayer(GameObject obj)
    {
        players.Add(obj);
    }

    public void RemovePlayer(GameObject obj)
    {
        players.Remove(obj);
    }
}
