using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    public enum ZombieType
    {
        Zombie,
    }

    private enum ZombieState
    {
        Idle,
        Move,
        Attack,
        Dead,
    }

    public ZombieType zType = ZombieType.Zombie;
    private ZombieState zState = ZombieState.Idle;

    public NavMeshAgent agent;
    public Animator anim;

    public GameObject target;
    public float detectionRange = 40;

    public float attackDistnace = 2.5f;
    public float zDamage = 15;

    public float maxHp = 100;
    public float currentHp = 100;
    public bool live;

    private void Start()
    {
        live = true;
    }
    private void Update()
    {
        FindTarget();
        AnimationUpdate();
    }

    private void AnimationUpdate()
    {
        switch (zState)
        {
            case ZombieState.Idle:
                Idle();
                break;
            case ZombieState.Move:
                Move();
                break;
            case ZombieState.Attack:
                Attack();
                break;
            case ZombieState.Dead:
                Dead();
                break;

        }
    }

    private void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float distance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            Vector3 dir = player.transform.position - transform.position;
            float distance_Player = dir.magnitude;

            // 감지 범위 내에 있는 플레이어만을 대상으로
            if (distance_Player <= detectionRange)
            {
                // Physics.RaycastAll을 사용하여 모든 충돌 지점을 가져옴
                RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, distance);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (distance_Player < distance)
                        {
                            distance = distance_Player;
                            closestPlayer = player;
                        }
                    }
                }
            }
        }

        target = closestPlayer;
        if (target != null && distance > attackDistnace)
        {
            zState = ZombieState.Move;
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
        }
    }

    private void Idle()
    {
        if (!live)
            return;
    }

    private void Move()
    {
        if (!live || target == null)
            return;

        agent.SetDestination(target.transform.position);

        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;

        if (distance <= attackDistnace)
        {
            zState = ZombieState.Attack;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            return;
        }

    }

    private void Attack()
    {
        if (!live)
            return;

        agent.velocity = Vector3.zero;

    }

    private void Hurt(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            live = false;
            zState = ZombieState.Dead;
        }
    }

    private void Dead()
    {
        if (live)
            return;

        anim.SetTrigger("Dead");
        Destroy(gameObject, 1.5f);
    }
}
