using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HiddenPortal : MonoBehaviour
{
    public ParticleSystem shPS;
    public float currentTime;
    public bool playing;

    private void Start()
    {
        playing = false;
        shPS.Pause();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        exxit();
    }

    /* private void OnCollisionEnter(Collision collision)
{
        // �ð��� 1120 ~ 1160 �� ���� ����
    if (currentTime >= 1120 && currentTime <= 1160)
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
        if (other.CompareTag("Player"))
        {
            pCheck--;
        }
    }
    */

    void exxit()
    {
        if (currentTime >= 20 && currentTime <= 1160)
        {
            playing = true;
            shPS.Play();
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
}
