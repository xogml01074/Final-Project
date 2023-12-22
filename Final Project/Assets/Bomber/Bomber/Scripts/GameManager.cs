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

    // 적 킬 수 카운트
    public void killCount()
    {
        ++enemyCount;
        countTxt.text = $"{enemyCount} Kill";
    }

    public void scoreCount()
    {
        int randomValue = 1;

        // 1 / 10 확률로 점수 3배 획득
        if (randomValue == 1)
        {
            score += 30;
            scoreTxt.text = $"Score : {score}";
            print("Rare Score");
        }

        // 아닐경우 일반 점수
        else
        {
            score += 10;
            scoreTxt.text = $"Score : {score}";
        }
    }
}
