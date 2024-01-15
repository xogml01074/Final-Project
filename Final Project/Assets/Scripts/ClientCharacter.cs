using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClientCharacter : MonoBehaviour
{
    public GameObject options;
    private bool optionsActive;

    private Slider bgm;
    private Slider sfx;

    private Button save;
    private Button close;

    private void Start()
    {

    }

    private void Update()
    {
        SetActiveOptions();
    }

    private void SetActiveOptions()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!optionsActive)
            {
                bgm.value = PlayerPrefs.GetFloat("BGMSound");
                sfx.value = PlayerPrefs.GetFloat("SFXSound");

                options.SetActive(true);
                optionsActive = true;
            }

            else
            {
                options.SetActive(false);
                optionsActive = false;
            }
        }
    }
    private void OnClickSave()
    {
        PlayerPrefs.Save();
    }

    private void OnclickClose()
    {
        options.SetActive(false);
        optionsActive = false;
    }
}
