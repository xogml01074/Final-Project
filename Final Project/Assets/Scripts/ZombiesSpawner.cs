using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiesSpawner : MonoBehaviour
{
    public BoxCollider rangeCollider;

    public List<GameObject> Zombies;
    public GameObject boss;

    public float spawnDelay = 3f;
    public float currentTime;
    public int count;

    private void Start()
    {
        rangeCollider = GetComponent<BoxCollider>();

        StartCoroutine(ZombieSpawn());
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        SetSpawnDelay();
    }

    IEnumerator ZombieSpawn()
    {
        int idx = 0;

        while (true)
        {
            // 일정 시간이 지나면 좀비의 종류를 증가시키는 코드
            if (count <= 5)
                idx = GetZombieType();

            Vector3 spawnPos = GetSpawnPoint();
            Instantiate(Zombies[idx], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SetSpawnDelay()
    {
        if (spawnDelay == 1f)
            return;

        if (currentTime <= 60)
        {
            spawnDelay -= 0.25f;
            currentTime = 0;
            count++;
        }
    }

    // 콜라이더 범위안에 랜덤한 스폰포인트를 생성해 반환하는 메소드
    private Vector3 GetSpawnPoint()
    {
        Vector3 basePos = transform.position;

        // 콜라이더의 사이즈를 가져오는 bounds.size 사용
        float rdX = rangeCollider.bounds.size.x;
        float rdZ = rangeCollider.bounds.size.z;

        rdX = Random.Range(-rdX / 2f, rdX / 2f);
        rdZ = Random.Range(-rdZ / 2f, rdZ / 2f);
        // 콜라이더 내부에서 랜덤한 위치 설정
        Vector3 spawnPos = new Vector3(rdX, 0, rdZ);
        // 기존 위치에 랜덤한 위치 플러스
        spawnPos += basePos;

        return spawnPos;
    }

    // 좀비 종류를 랜덤으로 변경하는 메소드
    private int GetZombieType()
    {
        int rd = Random.Range(0, 100);

        if (rd > 30)
            return 0;

        else
            return 1;
    }
}