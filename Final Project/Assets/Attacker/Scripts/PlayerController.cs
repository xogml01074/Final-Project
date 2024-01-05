using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{ 
    Idle,
    Move,
    Attack,
    Hurt,
    Die,
}

public enum FireMode
{ 
    One, 
    Fire,
}

public class PlayerController : MonoBehaviour
{
    // 플레이어 상태 변수
    public PlayerState playerState;

    // 발사 상태 변수
    public FireMode fireMode;

    // 플레이어 속도 변수
    public float moveSpeed = 4f;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    // 중력 변수
    public float gravity = -10f;

    public float yVelocity = 0;

    Animator anim;

    // 점프 확인 변수
    public bool isJump = false;

    public float jumpPower = 3f;

    public GameObject bulletHole;

    public GameObject firePoint;

    public GameObject[] eff_Flash;

    public float weapon01_Power = 3;

    public GameObject bullet;
    
    bool isFire = false;

    int delay = 0;

    public int setDelay = 50;

    public int reLoadDelay = 1000;

    public float hp = 100;

    public float maxHp = 100;

    public float power = 1;

    public Slider hpBar;

    public Text hpText;

    public float bulletMagarzion = 30;

    public Text magerzionText;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        playerState = PlayerState.Idle;
        fireMode = FireMode.One;

    }


    private void Update()
    {
        if (delay > 0)
            delay--;

        PlayerMoving();
        AnimationUpdate();
        ChangeFM();
        ReloadMagarzion();

        hpText.text = hp.ToString();
        magerzionText.text = bulletMagarzion.ToString() + "/30";

        if (!isFire)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (delay <= 0 && bulletMagarzion >= 1)
                {
                    bulletMagarzion--;
                    delay = setDelay;
                    
                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);

                    StartCoroutine(ShootEffectOn(0.05f));
                }
            }
        }
        if (isFire)
        {
            if (Input.GetMouseButton(0))
            {
                if (delay <= 0 && bulletMagarzion >= 1)
                {
                    bulletMagarzion--;
                    delay = setDelay;

                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                    
                    StartCoroutine(ShootEffectOn(0.05f));
                }
            }
        }

    }

    // 플레이어 이동 함수
    public void PlayerMoving()
    {
        // 입력 키를 받아온다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 플레이어가 죽었을때 움직임을 멈춘다.
        if (playerState != PlayerState.Die)
        {
            if (h != 0 || v != 0)
                playerState = PlayerState.Move;
            else
                playerState = PlayerState.Idle;
        }

        // 플레이어가 Move가 아닐경우 움직임을 멈춘다.
        if (playerState != PlayerState.Move)
            anim.SetBool("move", false);

        // 보는 방향을 설정한다.
        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        if (isJump && cc.collisionFlags == CollisionFlags.Below)
        { 
            yVelocity = 0;
            isJump = false;
        }
        
        // Space를 누르면 점프
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            anim.SetTrigger("jump");
            yVelocity = jumpPower;
            isJump = true;
        }

        // 이동중 애니메이션 변경
        if (anim.GetBool("move"))
        {
            // LeftShift를 누르는 중에는 달린다
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 7;
                anim.SetBool("run", true);
            }
            else
            {
                moveSpeed = 4;
                anim.SetBool("run", false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("left", true);
            }
            else
            {
                anim.SetBool("left", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("right", true);

            }
            else
            { 
                anim.SetBool("right", false);
            }

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("back", true);
                moveSpeed = 3;
            }
            else
            { 
                anim.SetBool("back", false);
                moveSpeed = 4;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (anim.GetBool("crouch"))
            {
                case false:
                    anim.SetBool("crouch", true);
                    break;
                case true:
                    anim.SetBool("crouch", false);
                    break;
            }
        }

    }

    public void AnimationUpdate()
    {
        switch (playerState)
        { 
            case PlayerState.Idle:
                //anim
                break;
            case PlayerState.Move:
                anim.SetBool("move", true);
                break;
        }
        
    }

    IEnumerator ShootEffectOn(float duration)
    {
        // 랜덤하게 숫자를 뽑는다.
        int num = Random.Range(0, eff_Flash.Length - 1);
        // 이펙트 오브젝트 배열에서 뽑힌 숫자에 해당하는 이펙트 오브젝트를 활성화한다.
        eff_Flash[num].SetActive(true);
        // 지정한 시간만큼 기다린다.
        yield return new WaitForSeconds(duration);
        // 이펙트 오브젝트를 다시 비활성화한다.
        eff_Flash[num].SetActive(false);
    }

    public void ChangeFM()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            switch (fireMode)
            {
                case FireMode.One:
                    fireMode = FireMode.Fire;
                    isFire = true;
                    break;
                case FireMode.Fire:
                    fireMode = FireMode.One;
                    isFire = false;
                    break;
            }
        }
    }

    public void ReloadMagarzion() // 재장전 함수
    {
        if (Input.GetKeyDown(KeyCode.R)) // R키를 누르면 재장전 한다.
        {
            anim.SetTrigger("reloading");
            delay = reLoadDelay;
            bulletMagarzion = 30;
            
        }
    }
}
