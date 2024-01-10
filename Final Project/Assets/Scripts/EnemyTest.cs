using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Animator enemyAnim;

    private void Start()
    {
        navEnemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Die();
        navEnemy.destination = player.transform.position;
        DistanceCheck();
        float dcc = Vector3.Distance(player.transform.position, enemy.transform.position);
        enemyAnim.SetFloat("Distance", dcc);
    }
    public void Die()
    {
        int rv = 1;
        // 적 피가 0 이하일때
        if (enemyHp <= 0)
        {
            if (rv == Random.Range(0, 8))
            {
                print("Unique Item Dropped!");
                GameObject uniqueItem = Instantiate(itemFactory);
                uniqueItem.transform.position = enemy.transform.position;
            }
            // 파괴
            Destroy(enemy);
        }
    }


    public void MoveSpeedValue()
    {
        if (gameObject.name == "skinless zombie" || gameObject.name == "Zombie")
        {
            navEnemy.speed = 1f;
        }
        else if (gameObject.name == "YE_Zombie")
        {
            navEnemy.speed = 2f;
        }
    }
    public void DistanceCheck()
    {
        float dcc = Vector3.Distance(player.transform.position, enemy.transform.position);
        navEnemy.isStopped = false;
        if (dcc > 20)
        {
            MoveSpeedValue();
        }
        else if (dcc < 7f && dcc > 1.5f)
        {
            navEnemy.speed = 2.5f;
        }
        else if (dcc <= 1.5f)
        {
            navEnemy.isStopped = true;
            Attack();
            return;
        }
    }

    public void Attack()
    {
        float dcc = Vector3.Distance(player.transform.position, enemy.transform.position);
        enemyAnim.SetTrigger("Attack");
        player.GetComponent<PlayerMove>().Damaged(3);
    }

    // 맞았을때 처리
    public void HitEnemy(int hitpower)
    {
        enemyHp -= hitpower;
        print("123");
    }
}
