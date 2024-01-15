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
            // ���� �ð��� ������ ������ ������ ������Ű�� �ڵ�
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

    // ���� ������ �������� �����ϴ� �޼ҵ�
    private int GetZombieType()
    {
        int rd = Random.Range(0, 100);

        if (rd > 30)
            return 0;

        else
            return 1;
    }
}