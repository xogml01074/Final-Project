using System.Collections;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    // ���� �� ���� ������ ������ Ŭ����, ���� �ð� �� ���� ���� ����
    // ���� ���� ���� ���� ����, �ݺ�
    // �÷��̾ ����� ��� ���� ��������

    public GameObject enemy;
    public int level;

    private void Start()
    {
        level = 1;
    }

    private void Update()
    {
        //levelUpdate();
    }

    void levelUpdate()
    {
        if (level == 1)
        {
            for (int i = 1; i <= 30; i++)
            {
                Debug.Log($"Level {i} Start");
                StartCoroutine(lvupDelay());
                if (i % 10 == 0)
                {
                    Debug.Log("Special Level Start");
                }
                if (level == i && GameManager.gm.enemyCount == i + (i * 10))
                {
                    Debug.Log($"Level {i} Clear");
                    level = i + 1;
                }
            }
        }
    }

    IEnumerator lvupDelay()
    {
        yield return new WaitForSeconds(30f);
        print("playing...");
    }
}
