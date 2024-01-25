using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Fusion;
using static UnityEngine.GraphicsBuffer;

public class TitanScript : NetworkBehaviour
{
    enum TitanState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    TitanState t_State;
    public float findDistance = 30f;
    public float attackDistance = 7.5f;
    public float moveSpeed = 5f;
    CharacterController tcc;
    float currentTime = 0;
    float attackDelay = 3f;
    public int attackPower = 30;
    public int maxHp = 300;
    public int currentHp = 300;
    public Animator titanAnim;
    public NavMeshAgent navTitan;

    public GameObject titan;
    GameObject nearplayer;
    public GameObject itemFactory;

    public override void Spawned()
    {
        t_State = TitanState.Idle;
        tcc = GetComponent<CharacterController>();
        navTitan = GetComponent<NavMeshAgent>();
    }

    public override void FixedUpdateNetwork()
    {
        switch (t_State)
        {
            case TitanState.Idle:
                // Idle();
                break;
            case TitanState.Move:
                FindTargetAndMove();
                break;
            case TitanState.Attack:
                Attack();
                break;
            case TitanState.Damaged:
                //Damaged();
                break;
            case TitanState.Die:
                //Die();
                break;
        }
    }

    private void FindTargetAndMove()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float distance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            Vector3 dir = player.transform.position - transform.position;
            float distancePlayer = dir.magnitude;
            titanAnim.SetFloat("Distance", distancePlayer);

            // Physics.RaycastAll을 사용하여 모든 충돌 지점을 가져옴
            RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, distance);

            // 반복문을 사용해 부딪힌 지점의 플레이어들의 거리만 계산후 타겟 설정
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

        nearplayer = closestPlayer;

        // 만약 타겟이 있지만 플레이어와의 거리가 공격 거리보다 멀다면
        if (nearplayer != null && distance > attackDistance)
        {
            titanAnim.SetTrigger("IdleToMove");
        }
        // 만약 모든 플레이어가 사망했다면
        else if (nearplayer == null)
        {
            navTitan.isStopped = true;
            t_State = TitanState.Idle;
            titanAnim.SetTrigger("MoveToIdle");
        }
        else if (Time.timeScale == 0)
        {
            StartCoroutine(DieProcess());
        }

        // Move() 합침
        // 만일, 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다.
        if (distance > findDistance)
        {
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            navTitan.isStopped = true;
            navTitan.ResetPath();

            // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            navTitan.stoppingDistance = attackDistance;

            // 내비게이션의 목적지를 플레이어의 위치로 설정한다.
            navTitan.destination = nearplayer.transform.position;
            
            // 거리가 15 이내면 속도 증가
            if (distance < 15)
            {
                navTitan.speed = 10f;
            }
            else
            {
                navTitan.speed = moveSpeed;
            }
        }
        else if (distance <= attackDistance)
        {
            t_State = TitanState.Attack;
            print("상태 전환 : Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓는다.
            currentTime = attackDelay;

            // 공격 대기 애니메이션 플레이
            titanAnim.SetTrigger("MoveToAttackDelay");

            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            navTitan.isStopped = true;
            navTitan.ResetPath();
        }
    }

    // 죽음 상태 함수
    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        tcc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);

        t_State = TitanState.Move;
        print("상태 전환 : Damaged -> Move");
    }

    void Attack()
    {
        // 만일 플레이어가 공격 범위 이내에 있다면 플레이어를 공격한다
        if (Vector3.Distance(transform.position, nearplayer.transform.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                AttackAction();
                currentTime = 0;
                // 공격 애니메이션 플레이
                titanAnim.SetTrigger("StartAttack");
                titanAnim.SetInteger("AttackIndex", Random.Range(0, 3));
            }
        }
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환한다(재추격 실시)
        else
        {
            t_State = TitanState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;

            // 이동 애니메이션 플레이
            titanAnim.SetTrigger("AttackToMove");
        }
    }

    // 플레이어의 스크립트의 데미지 처리 함수를 실행하기
    public void AttackAction()
    {
        if (nearplayer.name == "Bomber(Clone)")
        {
            nearplayer.GetComponent<CharacterMovement>().Hurt(attackPower);
        }
        else if (nearplayer.name == "Player_Attacker(Clone)")
        {
            nearplayer.GetComponent<PlayerController>().hit(attackPower);
        }
    }

    // 데미지 실행 함수
    public void HitEnemy(float hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태라면 아무런 처리도 하지 않고
        // 함수를 종료한다.
        if (t_State == TitanState.Damaged ||
            t_State == TitanState.Die)
        {
            return;
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        currentHp -= hitPower;

        // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
        navTitan.isStopped = true;
        navTitan.ResetPath();

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (currentHp > 0)
        {
            t_State = TitanState.Damaged;
            print("상태 전환 : Any state -> Damaged");

            // 피격 애니메이션을 플레이한다.
            titanAnim.SetTrigger("Damaged");

            Damaged();
        }
        // 그렇지 않다면 죽음 상태로 전환한다.
        else
        {
            t_State = TitanState.Die;
            print("상태 전환 : Any state -> Die");

            // 죽음 애니메이션을 플레이한다.
            titanAnim.SetTrigger("Die");

            Die();
        }
    }
    /* void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환한다.
        if (Vector3.Distance(transform.position, nearplayer.transform.position) < findDistance)
        {
            t_State = TitanState.Move;
            print("상태 전환 : Idle -> Move");

            // 이동 애니메이션으로 전환하기
            titanAnim.SetTrigger("IdleToMove");
        }
        else if (Vector3.Distance(transform.position, nearplayer.transform.position) < findDistance)
        {
            t_State = TitanState.Idle;
            titanAnim.SetTrigger("MoveToIdle");
        }
    } */
}
