using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // ȸ�� �ӵ� ����
    [SerializeField]
    private float rotSpeed = 300f;

    // ȸ�� �� ����
    float mx = 0;

    private void Update()
    {
        // ���콺 �¿� �Է��� �޴´�.
        float mouse_X = Input.GetAxis("Mouse X");

        // ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ų��.
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // ȸ�� �������� ��ü�� ȸ����Ų��.
        transform.eulerAngles = new Vector3(0, mx + 60, 0);

    }

}
