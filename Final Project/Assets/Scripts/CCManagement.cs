using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CCManagement : MonoBehaviour
{
    public TextMeshProUGUI TMP;
    public Button backButton;
    public Button homeButton;
    public GameObject infoPannel;
    public Button attackerBtn;
    public Button bomberBtn;
    public GameObject bomberPlus;
    public GameObject attackerPlus;


    private void Start()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(false);
        attackerPlus.SetActive(false);
    }

    // ����â ����(Attacker)
    public void openInfoA()
    {
        infoPannel.SetActive(true);
        attackerPlus.SetActive(true);
        bomberPlus.SetActive(false);
        TMP.text = "Attacker ����\nAttacker�� ���� �Ϲ� FPS���Ӱ� ���� ĳ���� �Դϴ�.\n����, �ܹ�, Ư�� ź�� �߻� �� �� ������, ���� �������� ĳ�����Դϴ�.\n�̵�, �޸���, ���� ��� �⺻���� ��ġ�̸� ��ݹ���� �ٲ� �� �ֽ��ϴ�.\n �Ѿ��� �� �� 3�������� ��������," +
            "�Ÿ��� Ư�� ź�� �������� �޶����� ����� �ֽ��ϴ�.\n \n \nAttacker Basic Speed : 7f, ��� ��� ���� : C";
    }

    // (Bomber)
    public void openInfoB()
    {
        infoPannel.SetActive(true);
        bomberPlus.SetActive(true);
        attackerPlus.SetActive(false);
        TMP.text = "Bomber ���� \nBomber�� ��ź �߻�⸦ ����ϴ� ĳ���� �Դϴ�.\n���� �������� �����ִ� ������ ���� �ֱ� �����ϸ�, �⵿���� �������� Ư¡�Դϴ�.\n���� źâ���� 6�߷� ������ �Ǿ�������, �� �򶧱��� ������ ���ϴ� �г�Ƽ�� �ֽ��ϴ�.\n" +
            "���� ������ ���ѵǸ�, �̵��⸦ ����� �� �ֽ��ϴ�.\n \n \nBomber Basic Speed : 7f, ��� ������ : 20, ��� ���� : 15f";
    }


    // �ڷ� ����(����â ����������)
    public void goBack()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(!false);
        attackerPlus.SetActive(false);
    }


    // ���� ����
    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

    // �����ϸ� �ٷ� ���� ����
    public void gameStart()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
