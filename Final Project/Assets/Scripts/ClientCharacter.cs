using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCharacter : MonoBehaviour
{
    public GameObject options;

    private void Update()
    {
        SetActiveOptions();
    }

    public void SetActiveOptions()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject canvas = GameObject.FindWithTag("Options");
            options = canvas.transform.GetChild(0).gameObject;

            if (options.activeSelf == false)
                options.SetActive(true);

            else
                options.SetActive(false);
        }
    }
}
