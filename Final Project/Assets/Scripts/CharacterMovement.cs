using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : NetworkBehaviour
{
    [SerializeField]
    private NetworkCharacterControllerPrototype cc;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Text nickNameTxt;

    // 닉네임 설정
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    NetworkString<_16> NickName { get; set; }

    [Networked]
    public NetworkButtons PrevButtons { get; set; }

    private NetworkButtons buttons;
    private NetworkButtons pressead;
    private NetworkButtons released;

    private Vector2 inputDir;
    private Vector3 moveDir;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            Destroy(cam.gameObject);
            return;
        }

        cam.gameObject.SetActive(true);
        RPC_SendNickName(UIManager.ui.inputNickName.text);
    }

    public override void Render()
    {
        // 나에게 입력권한이 없다면 리턴
        if (!Object.HasInputAuthority)
            return;

        // 카메라 회전 처리
        cam.transform.rotation = Quaternion.Euler(0, NetworkCallback.Nc.Yaw, 0);
        cam.transform.localRotation = Quaternion.Euler(NetworkCallback.Nc.Pitch, cam.transform.localRotation.y, cam.transform.localRotation.z);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;

        buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.buttons;
        }

        pressead = buttons.GetPressed(PrevButtons);
        released = buttons.GetReleased(PrevButtons);

        PrevButtons = buttons;

        inputDir = Vector2.zero;

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

        // 캐릭터 점프
        if (pressead.IsSet(Buttons.jump))
            cc.Jump();

        moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;
        moveDir.Normalize();

        cc.Move(moveDir);

        transform.rotation = Quaternion.Euler(0, (float)input.yaw, 0);
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
}
