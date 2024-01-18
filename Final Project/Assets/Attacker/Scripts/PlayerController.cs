using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{ 
    Idle,
    Move,
    Attack,
    Hurt,
    Dead,
}

// 사격모드
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
    
    // 사격 딜레이 변수
    float delay = 0;

    public float setDelay = 50f;

    public float reloadDelay = 1000f;

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

    public NetworkButtons PrevButtons { get; set; }

    private NetworkButtons buttons;
    private NetworkButtons pressead;
    private NetworkButtons released;

    private Vector2 inputDir;
    private Vector3 moveDir;

    public Text respawnTxt;


    public override void Spawned()
    {
        cc = GetComponent<CharacterController>();

        netCC = GetComponent<NetworkCharacterControllerPrototype>();

        if (Object.HasInputAuthority)
        {
            GameManager.gm.player_Attacker = this;

            GameManager.gm.pr = GetComponent<PlayerRotate>();
        }

        playerState = PlayerState.Idle;
        fireMode = FireMode.One;
    }

    private void Update()
    {
        ChangeFM();
        AnimationUpdate();

        if (delay > 0)
            delay--;
        if (rocketDelay > 0)
            rocketDelay -= Time.deltaTime;

    }

    public override void FixedUpdateNetwork()
    {
        if (playerState == PlayerState.Dead)
            return;

        // 키 입력값에 따른 실행
        buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.buttons;
        }

        pressead = buttons.GetPressed(PrevButtons);
        released = buttons.GetReleased(PrevButtons);

        PrevButtons = buttons;

        //hpText.text = hp.ToString();
        //magerzionText.text = bulletMagarzion.ToString() + "/30";
        //rocketBar.value = rocketDelay;
        //hpBar.value = hp;

        /*if (rocketDelay <= 0)
            rocketDelayText.text = "";
        else
            rocketDelayText.text = rocketDelay.ToString("0.00");*/

        if (fireMode == FireMode.One)    // 'fireMode'가 'One' 일 경우 마우스 왼쪽을 누르면 사격한다.
        {
            if (buttons.IsSet(Buttons.fire0))
            {
                if (delay <= 0 && bulletMagarzion >= 1)
                {
                    bulletMagarzion--;
                    delay = setDelay;

                    Runner.Spawn(bullet, firePoint.transform.position);

                    StartCoroutine(ShootEffectOn(0.05f));
                }
            }
        }
        if (fireMode == FireMode.Fire)     // 'fireMode'가 'Fire' 일 경우 마우스 왼쪽을 누르면 연사한다.
        {
            if (buttons.IsSet(Buttons.fire0))
            {
                if (delay <= 0 && bulletMagarzion >= 1)
                {
                    bulletMagarzion--;
                    delay = setDelay;

                    Runner.Spawn(bullet, firePoint.transform.position);

                    StartCoroutine(ShootEffectOn(0.05f));
                }
            }
        }

        if (buttons.IsSet(Buttons.fire1))        // 마우스 오른쪽을 누르면 로켓을 발사한다.
        {
            if (rocketDelay <= 0)
            {
                rocketDelay = setRocketDelay;

                Runner.Spawn(rocket, firePoint.transform.position);
            }
        }

        if (buttons.IsSet(Buttons.reload)) // R키를 누르면 재장전 한다.
        {
            anim.SetTrigger("reloading");
            delay = reloadDelay;
            bulletMagarzion = 30;
        }

        if (buttons.IsSet(Buttons.crouch))
            anim.SetBool("crouch", true);
        else
            anim.SetBool("crouch", false);
        

        PlayerMoving();
    }

    // 플레이어 이동 함수
    public void PlayerMoving()
    {
        if (playerState == PlayerState.Dead)
            return;

        // 입력 키를 받아온다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        if (h != 0 || v != 0)
            playerState = PlayerState.Move;
        else
            playerState = PlayerState.Idle;

        // 키 입력값에 따른 실행
        buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.buttons;
        }

        pressead = buttons.GetPressed(PrevButtons);
        released = buttons.GetReleased(PrevButtons);

        PrevButtons = buttons;

        inputDir = Vector2.zero;

        // 플레이어가 Move가 아닐경우 움직임을 멈춘다.
        if (playerState != PlayerState.Move)
            anim.SetBool("move", false);

        if (anim.GetBool("back") || anim.GetBool("crouch"))
            moveSpeed = 3;
        else if (anim.GetBool("run"))
            moveSpeed = 7;
        else
            moveSpeed = 4;

        // 이동중 애니메이션 변경
        if (anim.GetBool("move"))
        {
            // 버튼 입력값 설정
            if (buttons.IsSet(Buttons.run))
            {
                anim.SetBool("run", true);
            }
            else
                anim.SetBool("run", false);

            if (buttons.IsSet(Buttons.forward))
            {
                inputDir += Vector2.up;
            }
            if (buttons.IsSet(Buttons.back))
            {
                inputDir -= Vector2.up;
                anim.SetBool("back", true);
            }
            else
                anim.SetBool("back", false);

            if (buttons.IsSet(Buttons.right))
            {
                inputDir += Vector2.right;
                anim.SetBool("right", true);
            }
            else
                anim.SetBool("right", false);

            if (buttons.IsSet(Buttons.left))
            {
                inputDir -= Vector2.right;
                anim.SetBool("left", true);

            }
            else
                anim.SetBool("left", false);
        }
            
        if (buttons.IsSet(Buttons.crouch))
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

        // 캐릭터 점프
        if (pressead.IsSet(Buttons.jump))
            netCC.Jump();

        // 캐릭터 이동
        moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;
        moveDir.Normalize();

        netCC.Move(moveDir);

        transform.rotation = Quaternion.Euler(0, (float)input.yaw, 0);

        // 사망했을시 실행되는 리스폰 메소드
        CheckRespawn();
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
                    break;
                case FireMode.Fire:
                    fireMode = FireMode.One;
                    break;
            }
        }
    }

    public void hit(int damaged)
    {
        hp -= damaged;

    }

private void CheckRespawn()
{
    if (hp <= 0)
    {
        StartCoroutine(RespawnPlayer());
    }
}

IEnumerator RespawnPlayer()
{
    // 리스폰 쿨타임
    float ct = 15;
    ct -= Time.deltaTime;

    respawnTxt = GameObject.Find("RespawnText").GetComponent<Text>();
    respawnTxt.gameObject.SetActive(true);

    respawnTxt.text = string.Format($"사망하셨습니다.\n리스폰 까지 {(int)ct}");

    yield return new WaitForSeconds(15);

    if (ct <= 0)
    {
        respawnTxt.gameObject.SetActive(false);
        transform.position = SetPlayerSpawnPos.SetSpawnPosition();
        yield return null;
    }

    else
    {
        playerState = PlayerState.Dead;
        respawnTxt.text = string.Format($"게임 오버");
    }
}

}
