using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerManager : MonoBehaviour
{
    public List<GameObject> z_Spawners;
    
    public GameObject boss;
    public Transform bossSpawnPoint;

    private void Start()
    {
        StartCoroutine(nameof(SetAcitiveSpawner));
        StartCoroutine(nameof(BossSpawn));
    }

    // ���� ������ �ð��� Ȱ��ȭ �ڷ�ƾ
    IEnumerator SetAcitiveSpawner()
    {
        yield return new WaitForSeconds(15);

        z_Spawners[0].SetActive(true);

        yield return new WaitForSeconds(300);

        z_Spawners[1].SetActive(true);

        yield return new WaitForSeconds(300);

        z_Spawners[2].SetActive(true);
    }


    // ���� ��ȯ �ڷ�ƾ
    IEnumerator BossSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(280);

            Instantiate(boss, bossSpawnPoint.position, Quaternion.identity);
        }

    }
}
