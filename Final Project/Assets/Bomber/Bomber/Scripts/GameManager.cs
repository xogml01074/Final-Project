using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager gm;
    public int enemyCount;
    public Text countTxt;
    public int score;
    public Text scoreTxt;

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
        Run,
        Pause,
        GameOver
    }

    // ������ ���� ���� ����
    [Networked] public GameState gState { get; set; }

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    // PlayerMove Ŭ���� ����
    public PlayerMove player;

    // �ɼ� ȭ�� UI ������Ʈ ����
    public GameObject gameOption;

    public Slider hpSlider;
    public GameObject hitEffect;

    public List<GameObject> players;

    public PlayerRotate pr;

    public Text loginTime;
    public override void Spawned()
    {
        // �ʱ� ���� ���´� �غ� ���·� �����Ѵ�.
        gState = GameState.Run;

        // ���� ���� UI ������Ʈ���� Text ������Ʈ�� �����´�.
        gameText = gameLabel.GetComponent<Text>();

        // ���� �ؽ�Ʈ�� ������ ��Ȳ������ �Ѵ�.
        gameText.color = new Color32(255, 185, 0, 255);

        // ���� �غ� -> ���� �� ���·� ��ȯ�ϱ�
        StartCoroutine(ReadyToStart());

        // �÷��̾� ������Ʈ�� ã�� �� �÷��̾��� PlayerMove ������Ʈ �޾ƿ���
        //player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public override void FixedUpdateNetwork()
    {
        // ����, �÷��̾��� hp�� 0���϶��...
        if (player != null && player.hp <= 0)
        {
            // �÷��̾��� �ִϸ��̼��� �����.
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            // ���� �ؽ�Ʈ�� Ȱ��ȭ�Ѵ�.
            gameLabel.SetActive(true);

            // ���� �ؽ�Ʈ�� ������ 'Game Over'�� �Ѵ�.
            gameText.text = "Game Over";

            // ���� �ؽ�Ʈ�� ������ ���������� �Ѵ�.
            gameText.color = new Color32(255, 0, 0, 255);

            // ���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ������ ������Ʈ�� �����´�.
            Transform buttons = gameText.transform.GetChild(0);

            // ��ư ������Ʈ�� Ȱ��ȭ�Ѵ�.
            buttons.gameObject.SetActive(true);

            // ���¸� '���� ����' ���·� �����Ѵ�.
            gState = GameState.GameOver;
        }
    }

    IEnumerator ReadyToStart()
    {
        // 2�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(2f);

        // ���� �ؽ�Ʈ�� ������ "Go!"�� �Ѵ�.
        gameText.text = "Go!";

        // 0.5�ʰ�����Ѵ�.
        yield return new WaitForSeconds(0.5f);

        // ���� �ؽ�Ʈ�� ��Ȱ��ȭ�Ѵ�.
        gameLabel.SetActive(false);

        // ���¸� "���� ��" ���·� �����Ѵ�.
        gState = GameState.Run;
    }

    // �ɼ� ȭ�� �ѱ�
    public void OpenOptionWindow()
    {
        // �ɼ� â�� Ȱ��ȭ�Ѵ�.
        gameOption.SetActive(true);
        // ���� �ӵ��� 0������� ��ȯ�Ѵ�.
        Time.timeScale = 0f;
        // ���� ���¸� �Ͻ� ���� ���·� �����Ѵ�.
        gState = GameState.Pause;
    }

    // ����ϱ� �ɼ�
    public void CloseOptionWindow()
    {
        // �ɼ� â�� ��Ȱ��ȭ�Ѵ�.
        gameOption.SetActive(false);
        // ���� �ӵ��� 1������� ��ȯ�Ѵ�.
        Time.timeScale = 1f;
        // ���� ���¸� ���� �� ���·� �����Ѵ�.
        gState = GameState.Run;
    }

    // �ٽ��ϱ� �ɼ�
    public void RestartGame()
    {
        // ���� �ӵ��� 1������� ��ȯ�Ѵ�.
        Time.timeScale = 1f;
        // ���� �� ��ȣ�� �ٽ� �ε��Ѵ�.
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // �ε� ȭ�� ���� �ε��Ѵ�.
        SceneManager.LoadScene(1);
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


    /*private void Start()
    {
        enemyCount = 0;
        countTxt.text = $"{enemyCount} Kill";
        score = 0;
        scoreTxt.text = $"Score : {score}";
        // et = GameObject.FindWithTag("Enemy");
    }

    // �� ų �� ī��Ʈ
    public void killCount()
    {
        ++enemyCount;
        countTxt.text = $"{enemyCount} Kill";
    }

    public void scoreCount()
    {
        int randomValue = 1;

        // 1 / 10 Ȯ���� ���� 3�� ȹ��
        if (randomValue == 1)
        {
            score += 30;
            scoreTxt.text = $"Score : {score}";
            print("Rare Score");
        }

        // �ƴҰ�� �Ϲ� ����
        else
        {
            score += 10;
            scoreTxt.text = $"Score : {score}";
        }
    }*/
}
