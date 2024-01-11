using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerDataNetworked : NetworkBehaviour
{
    public string UserId { get; private set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            UserId = FindObjectOfType<PlayerData>().UserID;
        }
    }
}