using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HiddenPortal : MonoBehaviour
{
    public ParticleSystem shPS;
    public TextMeshProUGUI escapeTxt;
    public bool playing;
    public float currentTime;
    int pCheck = 0;

    private void Start()
    {
        playing = false;
        shPS.Pause();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        // 시간이 1120 ~ 1160 일 때만 열림
        if (currentTime >= 1120 && currentTime <= 1160)
        {
            playing = true;
            shPS.Play();
            // 인원수가 접속한 플레이어 수와 같다면
        }
        else if (currentTime < 1120)
        {
            playing = false;
            shPS.Pause();
        }
        else if (currentTime > 1160 && shPS.isPlaying)
        {
            playing = false;
            shPS.Pause();
            shPS.Stop();
        }
        else
        {
            Destroy(shPS);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentTime >= 1120 && currentTime <= 1160)
        {
            if (other.CompareTag("Player"))
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pCheck--;
        }
    }
}
