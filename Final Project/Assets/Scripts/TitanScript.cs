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

        nearplayer = closestPlayer;

        // ���� Ÿ���� ������ �÷��̾���� �Ÿ��� ���� �Ÿ����� �ִٸ�
        if (nearplayer != null && distance > attackDistance)
        {
            titanAnim.SetTrigger("IdleToMove");
        }
        // ���� ��� �÷��̾ ����ߴٸ�
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

        // Move() ��ħ
        // ����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�.
        if (distance > findDistance)
        {
            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            navTitan.isStopped = true;
            navTitan.ResetPath();

            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            navTitan.stoppingDistance = attackDistance;

            // ������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            navTitan.destination = nearplayer.transform.position;
            
            // �Ÿ��� 15 �̳��� �ӵ� ����
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
            print("���� ��ȯ : Move -> Attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������� ���´�.
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            titanAnim.SetTrigger("MoveToAttackDelay");

            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            navTitan.isStopped = true;
            navTitan.ResetPath();
        }
    }

    // ���� ���� �Լ�
    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        tcc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1f);

        t_State = TitanState.Move;
        print("���� ��ȯ : Damaged -> Move");
    }

    void Attack()
    {
        // ���� �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ �����Ѵ�
        if (Vector3.Distance(transform.position, nearplayer.transform.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                AttackAction();
                currentTime = 0;
                // ���� �ִϸ��̼� �÷���
                titanAnim.SetTrigger("StartAttack");
                titanAnim.SetInteger("AttackIndex", Random.Range(0, 3));
            }
        }
        // �׷��� �ʴٸ�, ���� ���¸� �̵�(Move)���� ��ȯ�Ѵ�(���߰� �ǽ�)
        else
        {
            t_State = TitanState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            titanAnim.SetTrigger("AttackToMove");
        }
    }

    // �÷��̾��� ��ũ��Ʈ�� ������ ó�� �Լ��� �����ϱ�
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

    // ������ ���� �Լ�
    public void HitEnemy(float hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���¶�� �ƹ��� ó���� ���� �ʰ�
        // �Լ��� �����Ѵ�.
        if (t_State == TitanState.Damaged ||
            t_State == TitanState.Die)
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        currentHp -= hitPower;

        // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        navTitan.isStopped = true;
        navTitan.ResetPath();

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        if (currentHp > 0)
        {
            t_State = TitanState.Damaged;
            print("���� ��ȯ : Any state -> Damaged");

            // �ǰ� �ִϸ��̼��� �÷����Ѵ�.
            titanAnim.SetTrigger("Damaged");

            Damaged();
        }
        // �׷��� �ʴٸ� ���� ���·� ��ȯ�Ѵ�.
        else
        {
            t_State = TitanState.Die;
            print("���� ��ȯ : Any state -> Die");

            // ���� �ִϸ��̼��� �÷����Ѵ�.
            titanAnim.SetTrigger("Die");

            Die();
        }
    }
    /* void Idle()
    {
        // ����, �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ�Ѵ�.
        if (Vector3.Distance(transform.position, nearplayer.transform.position) < findDistance)
        {
            t_State = TitanState.Move;
            print("���� ��ȯ : Idle -> Move");

            // �̵� �ִϸ��̼����� ��ȯ�ϱ�
            titanAnim.SetTrigger("IdleToMove");
        }
        else if (Vector3.Distance(transform.position, nearplayer.transform.position) < findDistance)
        {
            t_State = TitanState.Idle;
            titanAnim.SetTrigger("MoveToIdle");
        }
    } */
}
