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

    // ����â ����(Attacker)
    public void openInfoA()
    {
        infoPannel.SetActive(true);
        TMP.text = "Attacker ���� �׽�Ʈ";
    }

    // (Bomber)
    public void openInfoB()
    { 
        infoPannel.SetActive(true);
        TMP.text = "Bomber ���� �׽�Ʈ";
    }


    // �ڷ� ����(����â ����������)
    public void goBack()
    {
        infoPannel.SetActive(false);
    }


    // �α��� ������ ���ư���
    public void goHome()
    {
        SceneManager.LoadScene("LoginScene");
    }

    // �����ϸ� �ٷ� ���� ����
    public void gameStart()
    {
        SceneManager.LoadScene("Play");
    }
}
