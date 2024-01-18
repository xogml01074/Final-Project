using Fusion;
using System.Numerics;

enum Buttons 
{
    forward = 0,
    back = 1,
    right = 2,
    left = 3,
    jump = 4,
    run = 5,
    crouch = 6,
    reload = 7,
    fire0 = 8,
    fire1 = 9,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;

    public Angle yaw; // 가로축
    public Angle pitch; // 세로축

    public UnityEngine.Vector3 dir;
    public float mx;
}
