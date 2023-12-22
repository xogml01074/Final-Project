using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    public float rotSpeed = 300f;

    // ȸ�� �� ����
    float mx = 0;
    float my = 0;

    private void Update()
    {
        // ���콺 �Է��� �޴´�.
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ų��.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        // ���콺 ���� �̵� ȸ�� ����(my)�� ���� -90 ~90�� ���̷� �����Ѵ�
        my = Mathf.Clamp(my, -90f, 90f);

        // ��ü�� ȸ���������� ȸ����Ų��.
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }


}
