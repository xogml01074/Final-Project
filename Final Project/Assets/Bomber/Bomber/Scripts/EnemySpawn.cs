using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject enemyFactory;
    public GameObject gameLevel;


    private void Start()
    {
        spawnPoint.position = new Vector3 (Random.Range(-99, 99), 1, Random.Range(-99, 99));
    }
    /*private void Update()
    {
        if (GameLevel.GetCom)

        GameObject enemy = Instantiate(enemyFactory);
        enemy.transform.position = spawnPoint.position;
    }*/

    // 레벨에 따라 적 생성수가 달라짐
    // 높아질수록 적의 체력 / 수가 많아짐
}
