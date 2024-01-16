using Fusion;
using System.Numerics;

enum Buttons 
{
    forward = 0,
    back = 1,
    right = 2,
    left = 3,
    jump = 4,
    crouch = 5,
    reload = 6,
    fire0 = 7,
    fire1 = 8,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;

    public Angle yaw; // ������
    public Angle pitch; // ������

    public UnityEngine.Vector3 dir;
}
