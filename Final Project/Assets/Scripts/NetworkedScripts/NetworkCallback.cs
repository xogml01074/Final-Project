using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public PlayerRef playerRef;
    public NetworkObject playerObject;

    public Player()
    {

    }

    public Player(PlayerRef player, NetworkObject obj)
    {
        playerRef = player;
        playerObject = obj;
    }
}

public class NetworkCallback : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkCallback Nc;

    public NetworkRunner runner;

    public List<Player> runningPlayers = new List<Player>();
    public NetworkPrefabRef playerPrefab;

    private List<SessionInfo> _sessions = new List<SessionInfo>();

    private float yaw;
    public float Yaw 
    { 
        get { return yaw; } 
        set 
        {
            yaw = value;

            if (yaw < 0)
            {
                yaw = 360f;
            }

            if (yaw > 360)
            {
                yaw = 0;
            }
        } 
    }

    private float pitch;
    public float Pitch 
    { 
        get { return pitch; } 
        set 
        {
            pitch = value;

            pitch = Mathf.Clamp(pitch, -60, 80);
        }
    }

    private void Awake()
    {
        if (Nc == null)
        {
            Nc = this;
        }

        else
            Destroy(gameObject);
    }

    private void Update()
    {
        Yaw += Input.GetAxis("Mouse X");
        Pitch -= Input.GetAxis("Mouse Y");
    }
    public void ConnectToLobby(string playerName)
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }
        else
            return;

        runner.JoinSessionLobby(SessionLobby.Shared);
    }


    public void RefreshSessionListUI()
    {
        // 생성되어있는 세션 리스트 전부 삭제
        foreach (Transform child in UIManager.ui.sessionListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (SessionInfo session in _sessions)
        {
            if (session.IsVisible)
            {
                GameObject entry = GameObject.Instantiate(UIManager.ui.sessionPrefab, UIManager.ui.sessionListContent);
                Lobby lobby = entry.GetComponent<Lobby>();
                lobby.roomName.text = session.Name;
                lobby.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                {
                    lobby.joinBtn.interactable = false;
                }
                else
                {
                    lobby. joinBtn.interactable = true;
                }
            }
        }
    }
    public async void ConnectToSession(string roomName)
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        runner.ProvideInput = true;

        var gameArgs = new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            PlayerCount = 3,
        };

        await runner.StartGame(gameArgs);

        runner.SetActiveScene(1);
    }

    public async void CreateSession()
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        var gameArgs = new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = $"{UIManager.ui.inputNickName.text}'s room",
            PlayerCount = 3,
        };

        await runner.StartGame(gameArgs);

        runner.SetActiveScene(1);
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        _sessions.Clear();
        _sessions = sessionList;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var myInput = new NetworkInputData();

        myInput.buttons.Set(Buttons.forward, Input.GetKey(KeyCode.W));
        myInput.buttons.Set(Buttons.back, Input.GetKey(KeyCode.S));
        myInput.buttons.Set(Buttons.right, Input.GetKey(KeyCode.D));
        myInput.buttons.Set(Buttons.left, Input.GetKey(KeyCode.A));
        myInput.buttons.Set(Buttons.jump, Input.GetKey(KeyCode.Space));

        myInput.pitch = Pitch;
        myInput.yaw = Yaw;

        input.Set(myInput);
    }


    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!this.runner.IsServer)
            return;

        runningPlayers.Add(new Player(player, null));

        foreach (var players in runningPlayers)
        {
            if (players.playerObject != null)
                continue;

            var obj = this.runner.Spawn(playerPrefab, SetPlayerSpawnPos.SetSpawnPosition(), Quaternion.identity, players.playerRef);

            players.playerObject = obj;
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!this.runner.IsServer)
            return;

        foreach (var players in runningPlayers)
        {
            if (players.playerRef.Equals(player))
            {
                this.runner.Despawn(players.playerObject);
                runningPlayers.Remove(players);
            }
            break;
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (!this.runner.IsServer)
            return;

        foreach (var player in runningPlayers)
        {
            var obj = this.runner.Spawn(playerPrefab, SetPlayerSpawnPos.SetSpawnPosition(), Quaternion.identity, player.playerRef);

            player.playerObject = obj;
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        if(!this.runner.IsServer)
            return;

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
