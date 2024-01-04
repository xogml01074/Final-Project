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

    // 사용자 데이터를 새로 저장하거나 저장된 데이터를 읽어
    // 사용자의 입력과 일치하는지 검사하게 하고 싶다.

    // 유저 아이디 변수
    public InputField id;

    // 유저 패스워드 변수
    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    public Text usersList;

    private void Start()
    {
        // 검사 텍스트 창을 비운다.
        notify.text = "";
    }

    public void StartSharedSession()
    {
        string roomName =
            string.IsNullOrEmpty(_roomName.text) ? "BasicRoom" : _roomName.text;

        SetPlayerData();
        StartGame(GameMode.Shared, roomName, _gameSceneName);
    }

    private void SetPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        playerData.UserID = id.text;
    }

    private async void StartGame(GameMode mode, string roomName, string sceneName)
    {
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
            _runnerInstance = Instantiate(_networkRunnerPrefab);

        _runnerInstance.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            PlayerCount = 4,
        };

        StartGameResult res = await _runnerInstance.StartGame(startGameArgs);
        if (!res.Ok)
        {
            if (res.ShutdownReason == ShutdownReason.GameIsFull)
                notify.text = "방이 가득찼습니다.";
            else
                notify.text = res.ShutdownReason.ToString();

            return;
        }

        // _runnerInstance.SetActiveScene(sceneName);
    }

    // 아이디와 패스워드 저장 함수
    public void SaveUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료한다.
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        StartCoroutine(JoinDataPost(id.text, password.text));

        // 만일 시스템에 저장돼 있는 아이디가 존재하지 않는다면
        if (!PlayerPrefs.HasKey(id.text))
        {
            // 사용자의 아이디는 키(key)로 패스워드를 값(value)으로 설정해 저장한다.
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료됐습니다.";
        }
        // 그렇지 않으면, 이미 존재한다는 메시지를 출력한다.
        else
        {
            notify.text = "이미 존재하는 아이디입니다.";
        }
    }

    // 로그인 함수
    public void CheckUserData()
    {
        // 만일 입력 검사에 문제가 있으면 함수를 종료한다.
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        StartCoroutine(GetLastLoginTime(id.text));

        StartCoroutine(LoginDataPost(id.text, password.text));
    }

    IEnumerator GetLastLoginTime(string id)
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        string url = "http://localhost/fps_game/login_time.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", id);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                switch (www.downloadHandler.text)
                {
                    case "NULL":
                        break;
                    case "user not found":
                        break;
                    default:
                        playerData.LoginTime = www.downloadHandler.text;
                        break;
                }
            }
        }
    }

    IEnumerator LoginDataPost(string id, string password)
    {
        string url = "http://localhost/fps_game/login.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", id);
        form.AddField("passwordPost", password);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                switch (www.downloadHandler.text)
                {
                    case "login success":
                        StartSharedSession();
                        break;
                    case "password incorrect":
                        notify.text = "잘못된 비밀번호";
                        break;
                    case "user not found":
                        notify.text = "사용자 없음";
                        break;
                }
            }
            else
            {
                Debug.Log("error");
            }
        }
    }

    // 입력 완료 확인 함수
    bool CheckInput(string id, string pwd)
    {
        // 만일, 입력란이 하나라도 비어 있으면 유저 정보 입력을 요구한다.
        if (id == "" || pwd == "")
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요.";
            return false;
        }
        // 입력이 비어 있지 않으면 true를 반환한다.
        {
            return true;
        }
    }

    IEnumerator JoinDataPost(string id, string password)
    {
        string url = "http://localhost/fps_game/join.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", id);
        form.AddField("passwordPost", password);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                switch (www.downloadHandler.text)
                {
                    case "success":
                        notify.text = "아이디 생성이 완료되었습니다.";
                        break;
                    case "already exist":
                        notify.text = "이미 존재하는 아이디입니다.";
                        break;
                    case "fail":
                        notify.text = "아이디 생성이 실패했습니다.";
                        break;
                }
            }
            else
            {
                notify.text = "로그인 서버 접속 실패";
            }
        }
    }

    [System.Serializable]

    public class User
    {
        public int id;
        public string username;
    }

    public class Users
    {
        public User[] Items;
    }

    IEnumerator UserListPost()
    {
        string url = "http://localhost/fps_game/user_list.php";
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.error == null)
            {
                string jsonStr = "{\"Items\":" + www.downloadHandler.text + "}";
                Users users = JsonUtility.FromJson<Users>(jsonStr);
                string userStr = "";
                foreach (User user in users.Items)
                    userStr += $"ID : {user.id}, Name : {user.username}\n";

                usersList.text = userStr;
            }
        }
    }

    public void UserList()
    {
        StartCoroutine(UserListPost());
    }
}
