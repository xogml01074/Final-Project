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
    fire0One = 8,
    fire0Fire = 9,
    fire1 = 10,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;

    public Angle yaw; // ������
    public Angle pitch; // ������

    public UnityEngine.Vector3 dir;
    public float mx;
}
