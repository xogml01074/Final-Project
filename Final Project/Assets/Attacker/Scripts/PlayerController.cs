using Fusion;
using System.Collections;
using System.Collections.Generic;
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

public class PlayerController : NetworkBehaviour
{
    // 플레이어 상태 변수
    public PlayerState playerState;

    // 발사 상태 변수
    public FireMode fireMode;

    // 플레이어 속도 변수
    public float moveSpeed;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    // 중력 변수
    public float gravity = -10f;

    public float yVelocity = 0;

    public Animator anim;

    // 점프 확인 변수
    public bool isJump = false;

    public float jumpPower = 3f;

    // 사격 발사 위치 변수
    public GameObject firePoint;

    public GameObject[] eff_Flash;

    public float weapon01_Power = 3;

    public GameObject bullet;
    
    // 사격 모드 확인 변수
    bool isFire = false;

    // 사격 딜레이 변수
    int delay = 0;
    public int setDelay = 50;

    public int reloadDelay = 1000;

    public float hp = 100;

    public float maxHp = 100;

    public float power = 1;

    public Slider hpBar;

    public Text hpText;

    // 탄창 변수
    public float bulletMagarzion = 30;

    public Text magerzionText;

    // 로켓 변수
    public GameObject rocket;
    public Text rocketDelayText;
    public Slider rocketBar;

    // 로켓 딜레이 변수
    public float rocketDelay = 0;
    public float setRocketDelay = 5f;

    public Transform camPosition;

    NetworkCharacterControllerPrototype netCC;

    [Networked] private NetworkButtons _buttonsPrevious { get; set; }

    public override void Spawned()
    {
        cc = GetComponent<CharacterController>();

        netCC = GetComponent<NetworkCharacterControllerPrototype>();

        if (Object.HasInputAuthority)
        {
            GameManager.gm.player_Attacker = this;

            CamFollow cf = Camera.main.GetComponent<CamFollow>();
            cf.target = camPosition;

            GameManager.gm.pr = GetComponent<PlayerRotate>();
        }

        playerState = PlayerState.Idle;
        fireMode = FireMode.One;
    }


    public override void FixedUpdateNetwork()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            if (delay > 0)
                delay--;
            if (rocketDelay > 0)
                rocketDelay -= Time.deltaTime;

            //hpText.text = hp.ToString();
            //magerzionText.text = bulletMagarzion.ToString() + "/30";
            //rocketBar.value = rocketDelay;
            //hpBar.value = hp;

            /*if (rocketDelay <= 0)
                rocketDelayText.text = "";
            else
                rocketDelayText.text = rocketDelay.ToString("0.00");*/

            if (anim.GetBool("back") || anim.GetBool("crouch"))
                moveSpeed = 3;
            else if (anim.GetBool("run"))
                moveSpeed = 7;
            else
                moveSpeed = 4;

            if (GetInput(out NetworkInputData data))
            { 
                if (!isFire) // 'isFire'가 거짓일 경우 마우스 왼쪽을 누르면 사격한다.
                {
                    if (data.buttons.WasPressed(_buttonsPrevious, Buttons.fire0))
                    {
                        bulletMagarzion--;

                        Runner.Spawn(bullet);

                        StartCoroutine(ShootEffectOn(0.05f));
                    }
                }
                if (isFire) // 'isFire'가 참일 경우 마우스 왼쪽을 누르면 연사한다.
                {
                    if (data.buttons.WasPressed(_buttonsPrevious, Buttons.fire0))
                    {
                        if (delay <= 0 && bulletMagarzion >= 1)
                        {
                            bulletMagarzion--;
                            delay = setDelay;

                            Runner.Spawn(bullet);

                            StartCoroutine(ShootEffectOn(0.05f));
                        }
                    }
                }

                if (data.buttons.WasPressed(_buttonsPrevious, Buttons.fire1))
                {
                    if (rocketDelay <= 0)
                    {
                        rocketDelay = setRocketDelay;

                        Runner.Spawn(rocket);
                    }
                }
                _buttonsPrevious = data.buttons;
            }

            PlayerMoving();
            AnimationUpdate();
            ChangeFM();
            ReloadMagarzion();
        }
    }

    // 플레이어 이동 함수
    public void PlayerMoving()
    {
        // 입력 키를 받아온다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // 이동 방향을 설정한다.
        //Vector3 dir = new Vector3(h, 0, v);
        //dir = dir.normalized;

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
        //dir = Camera.main.transform.TransformDirection(dir);


        //dir.y = yVelocity;

        //cc.Move(dir * moveSpeed * Time.deltaTime);

        if (GetInput(out NetworkInputData data))
        {
            netCC.Move(data.dir * moveSpeed * Runner.DeltaTime);
            if (data.buttons.WasPressed(_buttonsPrevious, Buttons.Jump))
                netCC.Jump();

            _buttonsPrevious = data.buttons;
        }
        
        // Space를 누르면 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("jump");
        }

        // 이동중 애니메이션 변경
        if (anim.GetBool("move"))
        {
            // LeftShift를 누르는 중에는 달린다
            if (Input.GetKey(KeyCode.LeftShift))
                anim.SetBool("run", true);
            else
                anim.SetBool("run", false);

            if (Input.GetKey(KeyCode.A))
                anim.SetBool("left", true);
            else
                anim.SetBool("left", false);

            if (Input.GetKey(KeyCode.D))
                anim.SetBool("right", true);
            else
                anim.SetBool("right", false);

            if (Input.GetKey(KeyCode.S))
                anim.SetBool("back", true);
            else
                anim.SetBool("back", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
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

    public void ChangeFM() // 사격 모드 변경 함수
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
            delay = reloadDelay;
            bulletMagarzion = 30;
        }
    }
}
