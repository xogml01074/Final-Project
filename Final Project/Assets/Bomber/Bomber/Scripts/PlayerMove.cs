using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // 이동 속도 변수
    public float moveSpeed = 7f;

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

    public Animator playerAnim;

    private void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {

        // 키보드 W, A, S, D 키를 입력하면 캐릭터를 그 방향으로 이동시키고 싶다.

        // 1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        playerAnim.SetFloat("speedX", h);
        playerAnim.SetFloat("speedY", v);

        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);

        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프 전 상태로 초기화한다.
            isJumping = false;

            // 캐릭터 수직 속도를 0으로 만든다.
            yVelocity = 0;
        }

        // 2-3. 만일, 키보드 Spacebar 키를 눌렀다면...
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 속도에 점프력을 적용한다.
            yVelocity = jumpPower;
            isJumping = true;
            playerAnim.SetTrigger("Jump");
        }
        
        // 수구리기
        if (Input.GetKey(KeyCode.C))
        {
            playerAnim.SetTrigger("Crouched");
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
            {
                playerAnim.SetFloat("MoveMotion", dir.magnitude);
            }
        }

        // 2-4. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
