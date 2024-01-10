using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    public static GunFire gf;

    private void Awake()
    {
        if (gf == null)
        {
            gf = this;
        }
    }

    public enum GunMode
    {
        Pistol,
        Rifle,
    }

    public GunMode gMode;

    public Transform firePos_Rifle;
    public Transform firePos_Pistol;

    public GameObject crosshair_Pistol;
    public GameObject crosshair_Rifle;

    public float pistolDistance;
    public float rifleDistance;

    public float pistolRate = 0.5f;
    public float rifleRate = 0.1f;
    private float currentTime;

    public GameObject[] fireEffects;


    private void Update()
    {
        MouseInputCheck();
    }

    private void MouseInputCheck()
    {
        currentTime += Time.deltaTime;

        if (gMode == GunMode.Pistol && currentTime >= pistolRate)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire(0);
            }
        }

        if (gMode == GunMode.Rifle && currentTime >= rifleRate)
        {
            if (Input.GetMouseButton(0))
            {
                Fire(1);
            }
        }
    }

    private void Fire(int gmode)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        float distance = 0;

        if (gmode == 0)
            distance = pistolDistance;

        else if (gmode == 1)
            distance = rifleDistance;

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider != null)
                Debug.Log("무언가에 닿음");
        }

    }
}
