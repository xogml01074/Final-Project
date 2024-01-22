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
                // F Ű�� ���� Ż���̶�� ����
                escapeTxt.text = "Press F to Escape";
                // ������ Ż��
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
