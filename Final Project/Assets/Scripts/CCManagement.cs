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

    // 정보창 열기(Attacker)
    public void openInfoA()
    {
        infoPannel.SetActive(true);
        attackerPlus.SetActive(true);
        bomberPlus.SetActive(false);
        TMP.text = "Attacker 정보\nAttacker는 보통 일반 FPS게임과 같은 캐릭터 입니다.\n연사, 단발, 특수 탄을 발사 할 수 있으며, 가장 보편적인 캐릭터입니다.\n이동, 달리기, 점프 모두 기본적인 수치이며 사격방식을 바꿀 수 있습니다.\n 총알은 발 당 3데미지를 가졌으며," +
            "거리별 특수 탄의 데미지가 달라지는 기능이 있습니다.\n \n \nAttacker Basic Speed : 7f, 사격 방식 변경 : C";
    }

    // (Bomber)
    public void openInfoB()
    {
        infoPannel.SetActive(true);
        bomberPlus.SetActive(true);
        attackerPlus.SetActive(false);
        TMP.text = "Bomber 정보 \nBomber는 유탄 발사기를 사용하는 캐릭터 입니다.\n범위 데미지로 몰려있는 적에게 딜을 넣기 원할하며, 기동력이 느린것이 특징입니다.\n또한 탄창수도 6발로 제한이 되어있으며, 다 쏠때까지 장전을 못하는 패널티가 있습니다.\n" +
            "또한 점프는 제한되며, 이동기를 사용할 수 있습니다.\n \n \nBomber Basic Speed : 7f, 사격 데미지 : 20, 사격 범위 : 15f";
    }


    // 뒤로 가기(정보창 열려있을때)
    public void goBack()
    {
        infoPannel.SetActive(false);
        bomberPlus.SetActive(!false);
        attackerPlus.SetActive(false);
    }


    // 게임 종료
    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

    // 선택하면 바로 게임 시작
    public void gameStart()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
