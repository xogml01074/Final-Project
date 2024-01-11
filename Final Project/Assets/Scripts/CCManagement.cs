using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CCManagement : MonoBehaviour
{
    public TextMeshProUGUI TMP;
    public Button backButton;
    public Button homeButton;
    public GameObject infoPannel;
    public Button attackerBtn;
    public Button bomberBtn;


    private void Start()
    {
        infoPannel.SetActive(false);
    }

    // 정보창 열기(Attacker)
    public void openInfoA()
    {
        infoPannel.SetActive(true);
        TMP.text = "Attacker 정보 테스트";
    }

    // (Bomber)
    public void openInfoB()
    { 
        infoPannel.SetActive(true);
        TMP.text = "Bomber 정보 테스트";
    }


    // 뒤로 가기(정보창 열려있을때)
    public void goBack()
    {
        infoPannel.SetActive(false);
    }


    // 로그인 씬으로 돌아가기
    public void goHome()
    {
        SceneManager.LoadScene("LoginScene");
    }

    // 선택하면 바로 게임 시작
    public void gameStart()
    {
        SceneManager.LoadScene("Play");
    }
}
