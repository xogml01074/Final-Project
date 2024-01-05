using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TitanScript : MonoBehaviour
{
    public GameObject titan;
    public int attackPower = 15;
    public float moveSpeed = 5f;
    public int maxHp = 300;
    public int currentHp = 300;
    public NavMeshAgent navTitan;
    GameObject player;
    public Animator titanAnim;
    public GameObject itemFactory;

    private void Start()
    {
        navTitan = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        navTitan.speed = moveSpeed;
    }

    private void Update()
    {
        DistanceCheck();
        Die();
        float dcc = Vector3.Distance(player.transform.position, titan.transform.position);
        titanAnim.SetFloat("Distance", dcc);
        int hpValue = currentHp;
        titanAnim.SetInteger("Hp", hpValue);
    }

    public void DistanceCheck()
    {
        float dcc = Vector3.Distance(player.transform.position, titan.transform.position);
        navTitan.destination = player.transform.position;
        navTitan.isStopped = false;
        if (dcc < 30f && dcc > 8f)
        {
            navTitan.speed = 7.5f;
        }
        else if (dcc <= 8f)
        {
            Attack();
            return;
        }
    }

    public void Attack()
    {
        navTitan.isStopped = true;
        float dcc = Vector3.Distance(player.transform.position, titan.transform.position);
        titanAnim.SetInteger("AttackIndex", Random.Range(0, 3));
        titanAnim.SetTrigger("Attack");
        player.GetComponent<PlayerMove>().Damaged(attackPower);
    }

    public void Die()
    {
        if (currentHp <= 0)
        {
            GameObject uniqueItem = Instantiate(itemFactory);
            uniqueItem.transform.position = titan.transform.position;
            Destroy(titan);
        }
    }

    public void HitTitan(int hitpower)
    {
        currentHp -= hitpower;
        titanAnim.SetTrigger("GetHurt");
    }


}
