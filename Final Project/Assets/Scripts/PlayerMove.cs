using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
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

    // �ִϸ����� ����
    public Animator playerAnim;

    public Transform camPosition;

    [Networked] private NetworkButtons _buttonsPrevious { get; set; }

    //    public override void Spawned()
    private void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ִϸ����� �޾ƿ���
        playerAnim = GetComponentInChildren<Animator>();

        if (Object.HasInputAuthority)
        {
            GameManager.gm.player = this;
            // hpSlider = GameManager.gm.hpSlider;

            CamFollow cf = Camera.main.GetComponent<CamFollow>();
            cf.target = camPosition;

            GameManager.gm.pr = GetComponent<PlayerRotate>();
        }
    }

    //    public override void FixedUpdateNetwork()
    private void Update()
    {
        Move();
        if (hp <= 0)
        {
            Die();
        }
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
        //if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        //{
            // ���� �� ���·� �ʱ�ȭ�Ѵ�.
        //    isJumping = false;

            // ĳ���� ���� �ӵ��� 0���� �����.
        //    yVelocity = 0;
        //}

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnim.SetTrigger("Dodge");
        }
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        playerAnim.SetTrigger("Hit");
    }

    public void Die()
    {
        StopAllCoroutines();
        DieProcess();

    }

    IEnumerator DieProcess()
    {
        playerAnim.SetTrigger("Die");
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
