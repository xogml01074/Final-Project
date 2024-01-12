using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCheck : MonoBehaviour
{
    public int playerCnt = 0;

    public float currentTime = 0;

    private void Update()
    {
        currentTime += Time.deltaTime;

        ClearOrNot();
    }
    private void ClearOrNot()
    {
        if (currentTime < 1200)
        {
            GameManager.gm.gState = GameManager.GameState.GameOver;
            return;
        }

        else if (currentTime >= 1080)
        {
            if (InPlayerCheck())
                GameManager.gm.gState = GameManager.GameState.Clear;

            return;
        }
    }

    public bool InPlayerCheck()
    {
        if (playerCnt == GameManager.gm.players.Count)
            return true;

        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerCnt++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerCnt--;
    }
}