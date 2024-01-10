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

    // ���� ���̵� ����
    public InputField id;

    // ���� �н����� ����
    public InputField password;

    // �˻� �ؽ�Ʈ ����
    public Text notify;

    private void Start()
    {
        // �˻� �ؽ�Ʈ â�� ����.
        notify.text = "";
    }

    // ���ο� ������ �����ϴ� �ڵ�
    public void StartSharedSession()
    {
        string roomName =
            string.IsNullOrEmpty(_roomName.text) ? "BasicRoom" : _roomName.text;

        SetPlayerData();
        StartGame(GameMode.Shared, roomName, _gameSceneName);
    }

    // �÷��̾��� ������ ���� �޼ҵ�
    private void SetPlayerData()
    {
        PlayerData playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
            playerData = Instantiate(_playerDataPrefab);

        playerData.UserID = id.text;
    }

    // ���ӽ��� �޼ҵ�
    private async void StartGame(GameMode mode, string roomName, string sceneName)
    {
        // ���� �÷��� ������ �Ѿ�� �� ������ �񵿱�ó��
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
        // ���� ���� üũ
        if (!res.Ok)
        {
            // ������ ������ ������ ���� �� ������� "���� ����á���ϴ�." ���
            if (res.ShutdownReason == ShutdownReason.GameIsFull)
                notify.text = "���� ����á���ϴ�.";
            // �ƴ϶�� ������ ������� ���� ���� ���
            else
                notify.text = res.ShutdownReason.ToString();

            return;
        }

        // ���ӽ��� (���� ������ ����)
        _runnerInstance.SetActiveScene(sceneName);
    }

    public void CheckUserData()
    {
        if(!CheckInput(id.text, password.text))
            return;

        // ����ڰ� �Է��� ���̵� Key�� ����� ����� Value���� �Լ��� ����
        string pass = PlayerPrefs.GetString(id.text);

        // ���� ����� Value���� �Էµ� ��й�ȣ�� ��ġ�Ѵٸ� ���� ����
        if (password.text == pass)
            StartSharedSession();

        else
            notify.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
    }


    // ���̵� ��й�ȣ �Է�Ȯ�� �޼ҵ�
    private bool CheckInput(string id, string pwd)
    {
        // ���� �Է¶��� �ϳ��� ��� ������ ���� ���� �Է� �䱸
        if (id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� ��й�ȣ�� �Է����ּ���.";
            return false;
        }

        else
            return true;
    }

    // �÷��̾��� ���̵� ��й�ȣ ���� �޼ҵ�
    public void SaveUserDate()
    {
        // ���� ����Ǿ��ִ� ���̵� �������� ������ ���̵� ����
        if (!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�Ǿ����ϴ�.";
        }

        else
            notify.text = "�̹� �����ϴ� ���̵� �Դϴ�.";
    }
}
