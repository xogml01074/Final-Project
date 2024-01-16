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

    public int maxHP = 100;
    public int currentHP = 100;
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

    Animator playerAnim;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            Destroy(cam.gameObject);
            Destroy(miniMapCam);
            return;
        }
        
        cam.gameObject.SetActive(true);
        miniMapCam.SetActive(true);
        RPC_SendNickName(UIManager.ui.inputNickName.text);

        playerAnim = GetComponentInChildren<Animator>();
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

        // �Է°��� ���� �ִϸ��̼�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        playerAnim.SetFloat("speedY", h);
        playerAnim.SetFloat("speedX", v);


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

        // ��������� ����Ǵ� ������ �޼ҵ�
        CheckRespawn();
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

    private void CheckRespawn()
    {
        if (currentHP <= 0)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {
        // ������ ��Ÿ��
        float ct = 15;
        ct -= Time.deltaTime;

        respawnTxt = GameObject.Find("RespawnText").GetComponent<Text>();
        respawnTxt.gameObject.SetActive(true);

        respawnTxt.text = string.Format($"����ϼ̽��ϴ�.\n������ ���� {(int)ct}");

        yield return new WaitForSeconds(15);

        if (ct <= 0)
        {
            respawnTxt.gameObject.SetActive(false);
            transform.position = SetPlayerSpawnPos.SetSpawnPosition();
            yield return null;
        }

        else
        {
            ps = PlayerState.Dead;
            respawnTxt.text = string.Format($"���� ����");
        }
    }
}
