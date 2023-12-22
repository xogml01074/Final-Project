using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    public GameObject enemy;
    public float enemyMaxHp = 30;
    public float enemyHp = 30;
    public GameObject itemFactory;
    public NavMeshAgent navEnemy;
    GameObject player;

    private void Start()
    {
        navEnemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Die();
        navEnemy.destination = player.transform.position;
    }
    public void Die()
    {
        int rv = 1;
        // �� �ǰ� 0 �����϶�
        if (enemyHp <= 0)
        {
            GameManager.gm.killCount();
            GameManager.gm.scoreCount();
            if (rv == Random.Range(0, 8))
            {
                print("Unique Item Dropped!");
                GameObject uniqueItem = Instantiate(itemFactory);
                uniqueItem.transform.position = enemy.transform.position;
            }
            // �ı�
            Destroy(enemy);
        }
    }

    // �¾����� ó��
    public void HitEnemy(int hitpower)
    {
        enemyHp -= hitpower;
    }
}
