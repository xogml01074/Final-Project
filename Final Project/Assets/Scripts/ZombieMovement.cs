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

    private enum ZombieState
    {
        Idle,
        Move,
        Attack,
        Dead,
    }

    public ZombieType zType;
    private ZombieState zState;

    public NavMeshAgent agent;
    Animator anim;

    public GameObject target;

    public float attackDistnace = 2.5f;
    public float zDamage = 15;

    public float maxHp = 100;
    public float currentHp = 100;

    public void Start()
    {
        SetSpeedAndDamage();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
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
            Vector3 dir = player.transform.position - transform.position;
            float distancePlayer = dir.magnitude;

            // Physics.RaycastAll�� ����Ͽ� ��� �浹 ������ ������
            RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, distance);

            // �ݺ����� ����� �ε��� ������ �÷��̾���� �Ÿ��� ����� Ÿ�� ����
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (distancePlayer < distance)
                    {
                        distance = distancePlayer;
                        closestPlayer = player;
                    }
                }
            }
        }

        target = closestPlayer;

        // ���� Ÿ���� ������ �÷��̾���� �Ÿ��� ���� �Ÿ����� �ִٸ�
        if (target != null && distance > attackDistnace)
        {
            zState = ZombieState.Move;
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
        }
        // ���� ��� �÷��̾ ����ߴٸ�
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

        agent.SetDestination(target.transform.position);

        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;

        if (distance <= attackDistnace)
        {
            zState = ZombieState.Attack;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
        }

    }

    public void Attack()
    {
        if (zState == ZombieState.Dead)
            return;

        CharacterMovement attackT = target.GetComponent<CharacterMovement>();
        agent.velocity = Vector3.zero;
        attackT.currentHP -= zDamage;
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
        Destroy(gameObject, 1.5f);
    }
}
