using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiddenAdd : MonoBehaviour
{
    int pCheck = 0;
    public TextMeshProUGUI escapeTxt;
    public float currentTime;
    public GameObject gate;
    public bool isIn = false;

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        isIn = true;
        if (currentTime >= 20 && currentTime <= 1160)
        {
            pCheck++;
            if (pCheck == NetworkCallback.Nc.runningPlayers.Count)
            {
                // F 키를 눌러 탈출이라는 문구
                escapeTxt.text = "Press F to Escape";
                // 누르면 탈출
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.gm.gState = GameManager.GameState.Clear;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        escapeTxt.text = "";
        pCheck--;
    }

}
