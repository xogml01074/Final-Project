using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawnerManager : MonoBehaviour
{
    public List<GameObject> z_Spawners;
    
    public GameObject boss;
    public Transform bossSpawnPoint;

    public float spawnActiveCnt;
    public float bossSpawnerActiveCnt;

    private void Start()
    {
        //StartCoroutine(SetAcitiveSpawner());
        //StartCoroutine(BossSpawn());
    }

    private void Update()
    {
        spawnActiveCnt += Time.deltaTime;
        bossSpawnerActiveCnt = spawnActiveCnt;

        if (spawnActiveCnt >= 15f)
            z_Spawners[0].SetActive(true);
        if (spawnActiveCnt >= 300f)
        {
            z_Spawners[1].SetActive(true);
            z_Spawners[2].SetActive(true);
        }

        if (bossSpawnerActiveCnt >= 280)
        {
            bossSpawnerActiveCnt = 0f;
            Instantiate(boss, bossSpawnPoint.position, Quaternion.identity);
        }
    }

    // 좀비 스포너 시간별 활성화 코루틴
    IEnumerator SetAcitiveSpawner()
    {
        yield return new WaitForSeconds(15);

        z_Spawners[0].SetActive(true);

        yield return new WaitForSeconds(300);

        z_Spawners[1].SetActive(true);

        yield return new WaitForSeconds(300);

        z_Spawners[2].SetActive(true);
    }


    // 보스 소환 코루틴
    IEnumerator BossSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(280);

            Instantiate(boss, bossSpawnPoint.position, Quaternion.identity);
        }

    }
}
