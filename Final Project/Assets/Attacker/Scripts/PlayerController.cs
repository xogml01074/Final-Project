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
    // �÷��̾� ���� ����
    public PlayerState playerState;

    // �߻� ���� ����
    public FireMode fireMode;

    // �÷��̾� �ӵ� ����
    public float moveSpeed = 4f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �߷� ����
    public float gravity = -10f;

    public float yVelocity = 0;

    Animator anim;

    // ���� Ȯ�� ����
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

    // �÷��̾� �̵� �Լ�
    public void PlayerMoving()
    {
        // �Է� Ű�� �޾ƿ´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // �̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // �÷��̾ �׾����� �������� �����.
        if (playerState != PlayerState.Die)
        {
            if (h != 0 || v != 0)
                playerState = PlayerState.Move;
            else
                playerState = PlayerState.Idle;
        }

        // �÷��̾ Move�� �ƴҰ�� �������� �����.
        if (playerState != PlayerState.Move)
            anim.SetBool("move", false);

        // ���� ������ �����Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        if (isJump && cc.collisionFlags == CollisionFlags.Below)
        { 
            yVelocity = 0;
            isJump = false;
        }
        
        // Space�� ������ ����
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            anim.SetTrigger("jump");
            yVelocity = jumpPower;
            isJump = true;
        }

        // �̵��� �ִϸ��̼� ����
        if (anim.GetBool("move"))
        {
            // LeftShift�� ������ �߿��� �޸���
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
        // �����ϰ� ���ڸ� �̴´�.
        int num = Random.Range(0, eff_Flash.Length - 1);
        // ����Ʈ ������Ʈ �迭���� ���� ���ڿ� �ش��ϴ� ����Ʈ ������Ʈ�� Ȱ��ȭ�Ѵ�.
        eff_Flash[num].SetActive(true);
        // ������ �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(duration);
        // ����Ʈ ������Ʈ�� �ٽ� ��Ȱ��ȭ�Ѵ�.
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

    public void ReloadMagarzion() // ������ �Լ�
    {
        if (Input.GetKeyDown(KeyCode.R)) // RŰ�� ������ ������ �Ѵ�.
        {
            anim.SetTrigger("reloading");
            delay = reLoadDelay;
            bulletMagarzion = 30;
            
        }
    }
}
