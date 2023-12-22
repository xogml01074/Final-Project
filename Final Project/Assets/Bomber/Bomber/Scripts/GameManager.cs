using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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

    private void Start()
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
    }
}
