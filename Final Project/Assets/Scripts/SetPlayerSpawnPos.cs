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

        // �ݶ��̴� ���ο��� ������ ��ġ ����
        Vector3 spawnPos = new Vector3(rdX, 0, rdZ);

        // ���� ��ġ�� ������ ��ġ �÷���
        spawnPos += basePos;

        return spawnPos;
    }
}
