using Fusion;

enum Buttons 
{
    forward,
    back,
    right,
    left,
    jump,
    run,
    crouch,
    reload,
    fire0One,
    fire0Fire,
    fire1,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;

    public Angle yaw; // 가로축
    public Angle pitch; // 세로축
}
