using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CCManagement : MonoBehaviour
{
    public Text TMP;
    public GameObject infoPannel;
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
            "�Ÿ��� Ư�� ź�� �������� �޶����� ����� �ֽ��ϴ�.\n \n \nAttacker Basic Speed : 7f, ��� ��� ���� : F";
    }

    // (Bomber)
    public void openInfoB()
    {
        infoPannel.SetActive(true);
        bomberPlus.SetActive(true);
        attackerPlus.SetActive(false);
        TMP.text = "Bomber ���� \nBomber�� ��ź �߻�⸦ ����ϴ� ĳ���� �Դϴ�.\n���� �������� �����ִ� ������ ���� �ֱ� �����ϸ�, �⵿���� �������� Ư¡�Դϴ�.\n���� źâ���� 6�߷� ������ �Ǿ�������, �� �򶧱��� ������ ���ϴ� �г�Ƽ�� �ֽ��ϴ�.\n" +
            "5ȸ�� ��� ������ ���ڸ� ��� �� �� �ֽ��ϴ�.\n \n \n���� ������ : 100, ��� ������ : 20, ��� ���� : 15f";
    }


    // �ڷ� ����(����â ����������)
    public void goBack()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(!false);
        attackerPlus.SetActive(false);
    }

    // �����ϸ� �ٷ� ���� ����
    public void choiceAttacker()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(false);
        attackerPlus.SetActive(false);

        UIManager.ui.characterChoice = 0;
        UIManager.ui.cc.SetActive(false);
        UIManager.ui.menu.SetActive(true);
    }

    public void choiceBoomber()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(false);
        attackerPlus.SetActive(false);

        UIManager.ui.characterChoice = 1;
        UIManager.ui.cc.SetActive(false);
        UIManager.ui.menu.SetActive(true);
    }
}
