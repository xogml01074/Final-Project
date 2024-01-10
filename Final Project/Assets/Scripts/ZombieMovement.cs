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
    private ZombieState zState = ZombieState.Idle;

    public NavMeshAgent agent;
    public Animator anim;

    public GameObject target;

    public float attackDistnace = 2.5f;
    public float zDamage = 15;

    public float maxHp = 100;
    public float currentHp = 100;
    public bool live;

    private void Start()
    {
        live = true;
        SetSpeed();
    }
    private void Update()
    {
        FindTarget();
        AnimationUpdate();
    }

    private void SetSpeed()
    {
        if (zType == ZombieType.Zombie)
            agent.speed = 2;
        else if (zType == ZombieType.SkinlessZombie)
            agent.speed = 1.2f;
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

            // Physics.RaycastAll�� ����Ͽ� ��� �浹 ������ ������
            RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, distance);

            // �ݺ����� ����� �ε��� ������ �÷��̾���� �Ÿ��� ����� Ÿ�� ����
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

        target = closestPlayer;
        // ���� Ÿ���� ���ų� �÷��̾���� �Ÿ��� ���� �Ÿ����� �ִٸ�
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
            anim.SetBool("Idle", true);
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
            return;
        }

    }

    private void Attack()
    {
        if (zState == ZombieState.Dead)
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
            anim.SetTrigger("Dead");
        }
    }

    private void Dead()
    {
        if (live)
            return;

        StartCoroutine(DeadProcess());
    }

    IEnumerator DeadProcess()
    {
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}