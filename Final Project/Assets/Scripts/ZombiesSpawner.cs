using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiesSpawner : MonoBehaviour
{
    public BoxCollider rangeCollider;

    public GameObject Zombie;

    public float spawnDelay = 2.25f;
    public float currentTime;

    private void Start()
    {
        rangeCollider = GetComponent<BoxCollider>();

        StartCoroutine(nameof(ZombieSPawn));
    }

    private void Update()
    {
        SetSpawnDelay();
    }

    IEnumerator ZombieSPawn()
    {
        while (true)
        {
            Vector3 spawnPos = GetSpawnPoint();
            Instantiate(Zombie, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SetSpawnDelay()
    {
        if (spawnDelay == 1f)
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= 60)
        {
            spawnDelay -= 0.25f;
            currentTime = 0;
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
}