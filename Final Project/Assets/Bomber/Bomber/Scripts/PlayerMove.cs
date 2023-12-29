using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // �̵� �ӵ� ����
    public float moveSpeed = 2f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �߷� ����
    float gravity = -20f;

    // ���� �ӷ� ����
    public float yVelocity = 0;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ����
    public bool isJumping = false;

    // �÷��̾� ü�� ����
    public int hp = 20;

    public Animator playerAnim;

    private void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        // Ű���� W, A, S, D Ű�� �Է��ϸ� ĳ���͸� �� �������� �̵���Ű�� �ʹ�.

        // 1. ������� �Է��� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. �̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        playerAnim.SetFloat("speedX", h / 2);
        playerAnim.SetFloat("speedY", v / 2);

        // 2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2. ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // ���� �� ���·� �ʱ�ȭ�Ѵ�.
            isJumping = false;

            // ĳ���� ���� �ӵ��� 0���� �����.
            yVelocity = 0;
        }

        // 2-4. ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. �̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnim.SetFloat("speedX", h);
            playerAnim.SetFloat("speedY", v);
            cc.Move(dir * moveSpeed * Time.deltaTime * 2);
        }
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        // ��ġ�� �ִϸ��̼�
    }
}
