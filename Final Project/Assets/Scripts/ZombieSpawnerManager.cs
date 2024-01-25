using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerManager : NetworkBehaviour
{
    public List<GameObject> z_Spawners;
    
    public GameObject boss;
    public Transform bossSpawnPoint;

    public override void Spawned()
    {
        StartCoroutine(SetAcitiveSpawner());
        StartCoroutine(BossSpawn());
    }

    // 좀비 스포너 시간별 활성화 코루틴
    IEnumerator SetAcitiveSpawner()
    {
        yield return new WaitForSeconds(15);

        z_Spawners[0].SetActive(true);

        yield return new WaitForSeconds(150);

        z_Spawners[1].SetActive(true);

        yield return new WaitForSeconds(150);

        z_Spawners[2].SetActive(true);
    }


    // 보스 소환 코루틴
    IEnumerator BossSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(240);

            NetworkCallback.Nc.runner.Spawn(boss, bossSpawnPoint.position, Quaternion.identity);
        }

    }
}
