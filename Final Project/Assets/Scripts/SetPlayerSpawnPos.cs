using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetPlayerSpawnPos
{

    public static Vector3 SetSpawnPosition()
    {
        Vector3 basePos = new Vector3(580, 17.5f, 412);

        float rdX = Random.Range(-20, 20);
        float rdZ = Random.Range(-5, 5);

        // 콜라이더 내부에서 랜덤한 위치 설정
        Vector3 spawnPos = new Vector3(rdX, 0, rdZ);

        // 기존 위치에 랜덤한 위치 플러스
        spawnPos += basePos;

        return spawnPos;
    }
}
