using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public Text roomName;
    public Text playerCount;
    public Button joinBtn;

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    public void JoinSession()
    {
        UIManager.ui.lobby.SetActive(false);
        NetworkCallback.Nc.ConnectToSession(roomName.text);
    }
}
