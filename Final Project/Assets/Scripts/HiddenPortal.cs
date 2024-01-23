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

    void exxit()
    {
        if (currentTime >= 444 && currentTime <= 555)
        {
            playing = true;
            shPS.Play();
        }
        else if (currentTime < 444)
        {
            playing = false;
            shPS.Pause();
        }
        else if (currentTime > 555 && shPS.isPlaying)
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
