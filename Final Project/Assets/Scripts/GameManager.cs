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

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Start,
        GameOver,
        Clear,
    }

    // ������ ���� ���� ����
    [Networked] public GameState gState { get; set; }

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    // PlayerMove Ŭ���� ����
    public PlayerMove player;

    // PlayerController Ŭ���� ����
    public PlayerController player_Attacker;
    public List<GameObject> players;

    public PlayerRotate pr;

    public Text loginTime;
    public override void Spawned()
    {
        // �ʱ� ���� ���´� �غ� ���·� �����Ѵ�.
        gState = GameState.Ready;

        // ���� �غ� -> ���� �� ���·� ��ȯ�ϱ�
        StartCoroutine(ReadyToStart());
    }

    public override void FixedUpdateNetwork()
    {
        ClearOrNot();
    }

    IEnumerator ReadyToStart()
    {
        // 2�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(1f);

        // ���¸� "���� ��" ���·� �����Ѵ�.
        gState = GameState.Start;
    }

    private void ClearOrNot()
    {
        if (gState == GameState.Clear)
        {
            gameLabel.SetActive(true);
            gameText.text = "Ŭ����!";

            Time.timeScale = 0f;
        }

        else if (gState == GameState.GameOver)
        {
            gameLabel.SetActive(true);
            gameText.text = "�������";

            Time.timeScale = 0f;
        }
    }

    // ���� ���� �ɼ�
    public void QuitGame()
    {
        // ���ø����̼��� �����Ѵ�.
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
