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

    // ����� �����͸� ���� �����ϰų� ����� �����͸� �о�
    // ������� �Է°� ��ġ�ϴ��� �˻��ϰ� �ϰ� �ʹ�.

    // ���� ���̵� ����
    public InputField id;

    // ���� �н����� ����
    public InputField password;

    // �˻� �ؽ�Ʈ ����
    public Text notify;

    public Text usersList;

    private void Start()
    {
        // �˻� �ؽ�Ʈ â�� ����.
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
                notify.text = "���� ����á���ϴ�.";
            else
                notify.text = res.ShutdownReason.ToString();

            return;
        }

        // _runnerInstance.SetActiveScene(sceneName);
    }

    // ���̵�� �н����� ���� �Լ�
    public void SaveUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� �����Ѵ�.
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        StartCoroutine(JoinDataPost(id.text, password.text));

        // ���� �ý��ۿ� ����� �ִ� ���̵� �������� �ʴ´ٸ�
        if (!PlayerPrefs.HasKey(id.text))
        {
            // ������� ���̵�� Ű(key)�� �н����带 ��(value)���� ������ �����Ѵ�.
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�ƽ��ϴ�.";
        }
        // �׷��� ������, �̹� �����Ѵٴ� �޽����� ����Ѵ�.
        else
        {
            notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";
        }
    }

    // �α��� �Լ�
    public void CheckUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� �����Ѵ�.
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
                        notify.text = "�߸��� ��й�ȣ";
                        break;
                    case "user not found":
                        notify.text = "����� ����";
                        break;
                }
            }
            else
            {
                Debug.Log("error");
            }
        }
    }

    // �Է� �Ϸ� Ȯ�� �Լ�
    bool CheckInput(string id, string pwd)
    {
        // ����, �Է¶��� �ϳ��� ��� ������ ���� ���� �Է��� �䱸�Ѵ�.
        if (id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� �н����带 �Է����ּ���.";
            return false;
        }
        // �Է��� ��� ���� ������ true�� ��ȯ�Ѵ�.
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
                        notify.text = "���̵� ������ �Ϸ�Ǿ����ϴ�.";
                        break;
                    case "already exist":
                        notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";
                        break;
                    case "fail":
                        notify.text = "���̵� ������ �����߽��ϴ�.";
                        break;
                }
            }
            else
            {
                notify.text = "�α��� ���� ���� ����";
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
