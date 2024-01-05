using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject enemyFactory;
    public GameObject gameLevel;
    public int cnt = 1;


    private void Start()
    {
        spawnPoint.position = new Vector3 (Random.Range(-49, 49), 1, Random.Range(-49, 49));
    }
    private void Update()
    {
        cnt++;
        if (cnt != 40)
        {
            GameObject enemy = Instantiate(enemyFactory);
            enemy.transform.position = spawnPoint.position;
        }
        else if (cnt == 40)
        {
            Destroy(spawnPoint.gameObject);
        }
    }
    // ������ ���� �� �������� �޶���
    // ���������� ���� ü�� / ���� ������
}
