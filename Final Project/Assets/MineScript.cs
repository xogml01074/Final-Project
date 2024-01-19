using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public int mPower = 50;
    public float explosionRadius = 30f;
    public ParticleSystem mineParticle;
    public GameObject mine;
    public LayerMask enemyLayer;
    public float currentTime;

    private void Start()
    {
        mineParticle = GetComponent<ParticleSystem>();
        mineParticle.Pause();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // �浹���� �ʾƵ� ���� �ð� �Ŀ� �ڵ����� ����
        if (currentTime >= 10F)
        {
            Collider[] co = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < co.Length; i++)
            {
                co[i].GetComponent<ZombieMovement>().Hurt(mPower);
            }
            mineParticle.Play();
            Destroy(gameObject, 0.2f);
        }
    }
    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        print(collision);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
        // ���� �ȵ�
        for (int i = 0; i < cols.Length; ++i)
        {
            cols[i].GetComponent<ZombieMovement>().Hurt(mPower);
            // cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
        }

        mineParticle.Play();
        Destroy(gameObject, 0.2f);
    }
}
