using Fusion;
using UnityEngine;

enum Buttons 
{
    forward = 0,
    back = 1,
    right = 2,
    left = 3,
    jump = 4,
    Jump = 5,
    fire0 = 6,
    fire1 = 7,
}
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;

    public Angle yaw; // ������
    public Angle pitch; // ������

    public Vector3 dir;
    public float mx;
}
