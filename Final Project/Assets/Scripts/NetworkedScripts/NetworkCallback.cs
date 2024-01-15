using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public NetworkRunner runner = null;

    public List<Player> runningPlayers = new List<Player>();
    public NetworkPrefabRef playerPrefab;

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
            runner = gameObject.AddComponent<NetworkRunner>();
        }

        else
            Destroy(gameObject);
    }


    private void Start()
    {

    }

    private void Update()
    {
        Yaw += Input.GetAxis("Mouse X");
        Pitch -= Input.GetAxis("Mouse Y");
    }
    public async void RunGame(GameMode mode)
    {
        runner.ProvideInput = true;

        var gameArgs = new StartGameArgs
        {
            GameMode = mode,
            SessionName = $"{UIManager.ui.inputNickName.text}'s Room",
            PlayerCount = 3,
        };

        await runner.StartGame(gameArgs);

        StartCoroutine(ConnectingSession());
    }

<<<<<<< HEAD
=======
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

        StartCoroutine(ConnectingSession());
        
    }

    IEnumerator ConnectingSession()
    {
        Text lodingTxt = UIManager.ui.loding.GetComponentInChildren<Text>();
        lodingTxt.text = "Connecting...";
        UIManager.ui.loding.SetActive(true);

        yield return new WaitForSeconds(2);

        runner.SetActiveScene(1);

        UIManager.ui.loding.SetActive(false);

    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        _sessions.Clear();
        _sessions = sessionList;
    }

>>>>>>> 166c8dd6b818daeb139fb63dfd01ee0377b178b3
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

            var obj = this.runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, players.playerRef);

            players.playerObject = obj;

            var cc = obj.GetComponent<CharacterController>();
            cc.enabled = false;
            obj.transform.position = new Vector3(0, 10, 0);
            cc.enabled = true;
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
            var obj = this.runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player.playerRef);

            var cc = obj.GetComponent<CharacterController>();
            cc.enabled = false;
            obj.transform.position = new Vector3(0, 10, 0);
            cc.enabled = true;
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        if(!this.runner.IsServer)
            return;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
