using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkPrefabRef playerNetworkPrefab;
    public Transform[] spawnPoints;

    public void Spawned()
    {
        SpawnPlayer(Runner.LocalPlayer);
    }

    public void SpawnPlayer(PlayerRef player)
    { 
        int index = player % spawnPoints.Length;
        var spawnPosition = spawnPoints[index].position;

        var playerObject = Runner.Spawn(playerNetworkPrefab, spawnPosition, Quaternion.identity, player);
        Runner.SetPlayerObject(player, playerObject);
    }
}
