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
        Respawn,
        GameOver,
        End,
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

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다.
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 색상을 주황색으로 한다.
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());
    }

    public override void FixedUpdateNetwork()
    {
        // 만일, 플레이어의 hp가 0이하라면...
        if (player != null && player.hp <= 0 || player_Attacker != null && player_Attacker.hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춘다.
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            
            // 상태 텍스트를 활성화한다.
            gameLabel.SetActive(true);

            // 상태 텍스트의 내용을 'Game Over'로 한다.
            gameText.text = "Game Over";

            // 상태 텍스트의 색상을 붉은색으로 한다.
            gameText.color = new Color32(255, 0, 0, 255);

            // 상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져온다.
            Transform buttons = gameText.transform.GetChild(0);

            // 버튼 오브젝트를 활성화한다.
            buttons.gameObject.SetActive(true);

            // 상태를 '게임 오버' 상태로 변경한다.
            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        // 2초간 대기한다.
        yield return new WaitForSeconds(2f);

        // 상태 텍스트의 내용을 "Go!"로 한다.
        gameText.text = "게임 시작!";

        // 0.5초간대기한다.
        yield return new WaitForSeconds(0.5f);

        // 상태 텍스트를 비활성화한다.
        gameLabel.SetActive(false);

        // 상태를 "게임 중" 상태로 변경한다.
        gState = GameState.Start;
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
