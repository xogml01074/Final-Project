using Fusion;
using UnityEngine;

public class PlayerRotate : NetworkBehaviour
{
    // ȸ�� �ӵ� ����
    [SerializeField]
    private float rotSpeed = 300f;

    // ȸ�� �� ����
    float mx = 0;

    // public override void FixedUpdateNetwork()
    private void Update()
    {
        // ������� ���콺 �Է��� �޾� �÷��̾ ȸ����Ű�� �ʹ�.
        // 1. ���콺 �¿� �Է��� �޴´�.
        float mouse_X = Input.GetAxis("Mouse X");

        // 1-1. ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ų��.
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // 2. ȸ�� �������� ��ü�� ȸ����Ų��.
        transform.eulerAngles = new Vector3(0, mx, 0);

    }
}
