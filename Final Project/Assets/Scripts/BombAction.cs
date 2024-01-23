using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombAction : NetworkBehaviour
{
    // ����Ʈ �Ҹ� ��� ������
    // ����ź ������
    public int attackPower = 20;

    // ���� ȿ�� �ݰ�
    public float explosionRadius = 15f;

    public ParticleSystem bombParticle;

    public GameObject bomb;

    public LayerMask enemyLayer;

    public override void Spawned()
    {
        bombParticle = transform.GetChild(0).GetChild(11).GetComponent<ParticleSystem>();
    }

    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.

        for (int i = 0; i < cols.Length; ++i)
        {
            if (cols[i].gameObject.tag == "Enemy")
            {
                cols[i].GetComponent<ZombieMovement>().Hurt(attackPower);
            }
            else if (cols[i].gameObject.tag == "Boss")
            {
                cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
            }
        }
        bombParticle.Play();
        Destroy(gameObject, 0.5f);
    }
}
