using System.Collections;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    // 일정 적 수를 잡으면 레벨이 클리어, 일정 시간 후 다음 레벨 시작
    // 일정 레벨 이후 보스 등장, 반복
    // 플레이어가 사망할 경우 레벨 원점으로

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
