using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{

    // 이동 속도 변수
    public float moveSpeed = 2f;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    // 중력 변수
    float gravity = -20f;

    // 수직 속력 변수
    public float yVelocity = 0;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력 변수
    public int hp = 20;

    // 애니메이터 변수
    public Animator playerAnim;

    public Transform camPosition;

    [Networked] private NetworkButtons _buttonsPrevious { get; set; }

    //    public override void Spawned()
    private void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 애니메이터 받아오기
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
        // 키보드 W, A, S, D 키를 입력하면 캐릭터를 그 방향으로 이동시키고 싶다.

        // 1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        playerAnim.SetFloat("speedX", h / 2);
        playerAnim.SetFloat("speedY", v / 2);

        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        //if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        //{
            // 점프 전 상태로 초기화한다.
        //    isJumping = false;

            // 캐릭터 수직 속도를 0으로 만든다.
        //    yVelocity = 0;
        //}

        // 2-4. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. 이동 속도에 맞춰 이동한다.
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
        print("소멸!");
        Destroy(gameObject);
    }
}
