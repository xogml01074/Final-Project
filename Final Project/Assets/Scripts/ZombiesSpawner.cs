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

    // �ݶ��̴� �����ȿ� ������ ��������Ʈ�� ������ ��ȯ�ϴ� �޼ҵ�
    private Vector3 GetSpawnPoint()
    {
        Vector3 basePos = transform.position;

        // �ݶ��̴��� ����� �������� bounds.size ���
        float rdX = rangeCollider.bounds.size.x;
        float rdZ = rangeCollider.bounds.size.z;

        rdX = Random.Range(-rdX / 2f, rdX / 2f);
        rdZ = Random.Range(-rdZ / 2f, rdZ / 2f);
        // �ݶ��̴� ���ο��� ������ ��ġ ����
        Vector3 spawnPos = new Vector3(rdX, 0, rdZ);
        // ���� ��ġ�� ������ ��ġ �÷���
        spawnPos += basePos;

        return spawnPos;
    }
}