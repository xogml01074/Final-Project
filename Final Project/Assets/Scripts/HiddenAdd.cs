using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (currentTime >= 444 && currentTime <= 555)
        {
            pCheck++;
            if (pCheck == NetworkCallback.Nc.runningPlayers.Count)
            {
                Color color = new Color32(153, 102, 255, 255);
                escapeTxt.color = color;
                escapeTxt.text = "HIDDEN PORTAL ENDING!\n THE END";
                Time.timeScale = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        escapeTxt.text = "";
        pCheck--;
    }
}
