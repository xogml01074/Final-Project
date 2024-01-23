using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : NetworkBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Dead,
    }

    public PlayerState ps = PlayerState.Idle;
    [SerializeField]
    private NetworkCharacterControllerPrototype cc;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject miniMapCam;

    [SerializeField]
    private Text nickNameTxt;

    public float maxHP = 100;
    public float currentHP = 100;

    public float ct;
    public Text respawnTxt;

    // �г��� ����
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    NetworkString<_16> NickName { get; set; }

    [Networked]
    public NetworkButtons PrevButtons { get; set; }

    private NetworkButtons buttons;
    private NetworkButtons pressead;
    private NetworkButtons released;

    private Vector2 inputDir;
    private Vector3 moveDir;

    public Animator playerAnim;

    public override void Spawned()
    {
        playerAnim = GetComponentInChildren<Animator>();

        if (!Object.HasInputAuthority)
        {
            Destroy(cam.gameObject);
            Destroy(miniMapCam);
            return;
        }

        cam.gameObject.SetActive(true);
        miniMapCam.SetActive(true);
        RPC_SendNickName(UIManager.ui.inputNickName.text);

    }

    public override void Render()
    {
        // ������ �Է±����� ���ٸ� ����
        if (!Object.HasInputAuthority)
            return;

        // ī�޶� ȸ�� ó��
        cam.transform.rotation = Quaternion.Euler(0, NetworkCallback.Nc.Yaw, 0);
        cam.transform.localRotation = Quaternion.Euler(NetworkCallback.Nc.Pitch, cam.transform.localRotation.y, cam.transform.localRotation.z);
    }

    public override void FixedUpdateNetwork()
    {
        if (ps == PlayerState.Dead)
            return;

        // Ű �Է°��� ���� ����
        buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.buttons;
        }

        pressead = buttons.GetPressed(PrevButtons);
        released = buttons.GetReleased(PrevButtons);

        PrevButtons = buttons;

        inputDir = Vector2.zero;

        // ��ư �Է°� ����
        if (buttons.IsSet(Buttons.forward))
        {
            inputDir += Vector2.up;
        }
        if (buttons.IsSet(Buttons.back))
        {
            inputDir -= Vector2.up;
        }
        if (buttons.IsSet(Buttons.right))
        {
            inputDir += Vector2.right;
        }
        if (buttons.IsSet(Buttons.left))
        {
            inputDir -= Vector2.right;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Object.HasInputAuthority)
        {
            // �Է°��� ���� �ִϸ��̼�
            playerAnim.SetFloat("speedY", h);
            playerAnim.SetFloat("speedX", v);
        }


        // ĳ���� ����
        if (pressead.IsSet(Buttons.jump))
        {
            cc.Jump();
            playerAnim.SetTrigger("Jump");
        }

        // ĳ���� �̵�
        moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;
        moveDir.Normalize();

        cc.Move(moveDir);

        transform.rotation = Quaternion.Euler(0, (float)input.yaw, 0);

        // ��������� ����Ǵ� ������ �ڷ�ƾ
        if (currentHP <= 0)
            RespawnCheck();
    }

    public static void OnNickNameChanged(Changed<CharacterMovement> changed)
    {
        changed.Behaviour.SetNickName();
    }

    public void SetNickName()
    {
        nickNameTxt.text = NickName.Value;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SendNickName(NetworkString<_16> message)
    {
        NickName = message;
    }

    private void RespawnCheck()
    {
        respawnTxt = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Text>();
        respawnTxt.gameObject.SetActive(true);
        respawnTxt.text = string.Format($"����ϼ̽��ϴ�.\n������ ���� {(int)ct}��");

        StartCoroutine(RespawnPlayer());

        if (ct <= 0)
        {
            respawnTxt.gameObject.SetActive(false);
            transform.position = SetPlayerSpawnPos.SetSpawnPosition();
            return;
        }

        else
        {
            ps = PlayerState.Dead;
            respawnTxt.text = string.Format($"���� ����");
        }
    }

    IEnumerator RespawnPlayer()
    {
        // ������ ��Ÿ��
        ct = 15;
        ct -= Time.deltaTime;

        yield return new WaitForSeconds(15);
    }
}
