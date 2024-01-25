using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : NetworkBehaviour
{
    public enum ZombieType
    {
        Zombie,
        SkinlessZombie,
    }

    public enum ZombieState
    {
        Idle,
        Move,
        Attack,
        Dead,
    }

    public ZombieType zType;
    public ZombieState zState;

    public NavMeshAgent agent;
    Animator anim;

    public GameObject target;

    public float attackDistnace = 2.5f;
    public float zDamage = 15;

    public float maxHp = 100;
    public float currentHp = 100;

    public GameObject itemFactory;

    public override void Spawned()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        SetSpeedAndDamage();
    }

    public override void FixedUpdateNetwork()
    {
        AnimationUpdate();
        FindTarget();
    }

    private void SetSpeedAndDamage()
    {
        if (zType == ZombieType.Zombie)
            agent.speed = 2.2f;

        else if (zType == ZombieType.SkinlessZombie)
        {
            zDamage = 30;
            agent.speed = 1.4f;
        }
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
            float distancePlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distancePlayer < distance)
            {
                distance = distancePlayer;
                closestPlayer = player;
            }
        }

        target = closestPlayer;

        // 만약 타겟이 있지만 플레이어와의 거리가 공격 거리보다 멀다면
        if (target != null && distance > attackDistnace)
        {
            zState = ZombieState.Move;
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
        }
        // 만약 모든 플레이어가 사망했다면
        else if (target == null)
            zState = ZombieState.Idle;
    }

    private void Idle()
    {
        if (zState == ZombieState.Dead)
            return;

        if (zType == ZombieType.Zombie)
        {
            anim.SetBool("Idle", true);
            FindTarget();
        }

        else
        {
            FindTarget();
        }
    }

    private void Move()
    {
        if (zState == ZombieState.Dead || target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= attackDistnace)
        {
            zState = ZombieState.Attack;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            return;
        }

        agent.SetDestination(target.transform.position);
    }

    public void Attack()
    {
        if (zState == ZombieState.Dead)
            return;

        agent.isStopped = true;
        if (target.gameObject.name == "Bomber(Clone)")
        {
            CharacterMovement attackT = target.GetComponent<CharacterMovement>();
            attackT.currentHP -= zDamage;
        }
    }

    public void Hurt(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
            zState = ZombieState.Dead;
    }

    private void Dead()
    {
        agent.velocity = Vector3.zero;
        anim.SetTrigger("Dead");

        int rnd = 1;
        if (rnd == Random.Range(0, 25))
        {
            GameObject item = Instantiate(itemFactory);
            item.transform.position = transform.position;
        }
        Runner.Despawn(Object);
    }
}
