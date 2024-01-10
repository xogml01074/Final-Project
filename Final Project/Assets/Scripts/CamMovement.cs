using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Transform target;

    public float rotateSpeed = 150f;

    private float mx = 0;
    private float my = 0;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        CamFollow();
        CamRotate();
    }

    private void CamFollow()
    {
        transform.position = target.position;
    }

    private void CamRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        mx -= mouseY;
        my += mouseX;
        mx = Mathf.Clamp(mx, -58f, 80f);

        transform.eulerAngles = new Vector3(mx, my, 0); 
    }
}
