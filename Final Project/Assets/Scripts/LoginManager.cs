using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fusion;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
    [SerializeField] private PlayerData _playerDataPrefab = null;
    [SerializeField] private InputField _roomName = null;
    [SerializeField] private string _gameSceneName = null;

    private NetworkRunner _runnerInstance = null;

    // 유저 아이디 변수
    public InputField id;

    // 유저 패스워드 변수
    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    private void Start()
    {
        // 검사 텍스트 창을 비운다.
        notify.text = "";
    }

    // 새로운 세션을 생성하는 코드
    public void StartSharedSession()
    {
        string roomName =
            string.IsNullOrEmpty(_roomName.text) ? "BasicRoom" : _roomName.text;

        SetPlayerData();
        StartGame(GameMode.Shared, roomName, _gameSceneName);
    }

    // 플레이어의 데이터 관리 메소드
    private void SetPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        playerData.UserID = id.text;
    }

    // 게임시작 메소드
    private async void StartGame(GameMode mode, string roomName, string sceneName)
    {
        // 게임 플레이 씬으로 넘어가기 전 까지의 비동기처리
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
            _runnerInstance = Instantiate(_networkRunnerPrefab);

        _runnerInstance.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            PlayerCount = 3,
        };

        await _runnerInstance.StartGame(startGameArgs);

        StartGameResult res = await _runnerInstance.StartGame(startGameArgs);
        // 접속 실패 체크
        if (!res.Ok)
        {
            // 접속이 실패한 이유가 방이 다 차서라면 "방이 가득찼습니다." 출력
            if (res.ShutdownReason == ShutdownReason.GameIsFull)
                notify.text = "방이 가득찼습니다.";
            // 아니라면 게임이 실행되지 않은 이유 출력
            else
                notify.text = res.ShutdownReason.ToString();

            return;
        }

        // 게임시작 (다음 씬으로 변경)
        _runnerInstance.SetActiveScene(sceneName);
    }

    public void CheckUserData()
    {
        if(!CheckInput(id.text, password.text))
            return;

        // 사용자가 입력한 아이디를 Key로 사용해 저장된 Value값을 함수에 저장
        string pass = PlayerPrefs.GetString(id.text);

        // 만약 저장된 Value값과 입력된 비밀번호가 일치한다면 세션 실행
        if (password.text == pass)
            StartSharedSession();

        else
            notify.text = "입력하신 아이디와 패스워드가 일치하지 않습니다.";
    }


    // 아이디 비밀번호 입력확인 메소드
    private bool CheckInput(string id, string pwd)
    {
        // 만일 입력란이 하나라도 비어 있으면 유저 정보 입력 요구
        if (id == "" || pwd == "")
        {
            notify.text = "아이디 또는 비밀번호를 입력해주세요.";
            return false;
        }

        else
            return true;
    }

    // 플레이어의 아이디 비밀번호 저장 메소드
    public void SaveUserDate()
    {
        // 만약 저장되어있는 아이디가 존재하지 않을시 아이디 생성
        if (!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료되었습니다.";
        }

        else
            notify.text = "이미 존재하는 아이디 입니다.";
    }
}
