using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerManager : NetworkBehaviour
{
    public List<GameObject> z_Spawners;
    
    public GameObject boss;
    public Transform bossSpawnPoint;

    public NetworkRunner runner;

    public override void Spawned()
    {
        runner = GameObject.Find("NetworkCallback").GetComponent<NetworkRunner>();
        StartCoroutine(SetAcitiveSpawner());
        StartCoroutine(BossSpawn());
    }

    // ���� ������ �ð��� Ȱ��ȭ �ڷ�ƾ
    IEnumerator SetAcitiveSpawner()
    {
        yield return new WaitForSeconds(15);

        z_Spawners[0].SetActive(true);

        yield return new WaitForSeconds(150);

        z_Spawners[1].SetActive(true);

        yield return new WaitForSeconds(150);

        z_Spawners[2].SetActive(true);
    }


    // ���� ��ȯ �ڷ�ƾ
    IEnumerator BossSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(240);

            runner.Spawn(boss, bossSpawnPoint.position, Quaternion.identity);
        }

    }
}
