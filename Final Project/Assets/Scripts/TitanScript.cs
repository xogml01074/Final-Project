using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TitanScript : MonoBehaviour
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
    public int attackPower = 15;
    public int maxHp = 300;
    public int currentHp = 300;
    public Slider hpSlider;
    public Animator titanAnim;
    public NavMeshAgent navTitan;

    public GameObject titan;
    GameObject nearplayer;
    public GameObject itemFactory;

    private void Start()
    {
        t_State = TitanState.Idle;
        tcc = GetComponent<CharacterController>();
        navTitan = GetComponent<NavMeshAgent>();
        nearplayer = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        switch (t_State)
        {
            case TitanState.Idle:
                Idle();
                break;
            case TitanState.Move:
                Move();
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

        hpSlider.value = (float)currentHp / maxHp;

        float dcc = Vector3.Distance(nearplayer.transform.position, titan.transform.position);
        titanAnim.SetFloat("Distance", dcc);
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
        // ������ ���� �Լ� ����
        // player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Move()
    {
        float minDistance = float.MaxValue;
        float distance = Vector3.Distance(transform.position, nearplayer.transform.position);
        if (minDistance > distance)
        {
            minDistance = distance;
        }

        // ����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�.
        if (minDistance > attackDistance)
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
        else
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

    void Idle()
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
    }

    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
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






    /*
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

    */
}
