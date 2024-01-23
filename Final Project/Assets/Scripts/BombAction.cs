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

    public LayerMask enemyLayer;

    public override void Spawned()
    {
        bombParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Zombie1(Clone)" || collision.gameObject.name == "Zombie2(Clone)")
        {
            // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].gameObject.GetComponentInChildren<ZombieMovement>().Hurt(attackPower);
            }
            bombParticle.Play();
            Destroy(gameObject, 0.5f);
        }
        else if (collision.gameObject.name == "Titan(Clone)")
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i].GetComponentInChildren<TitanScript>().HitEnemy(attackPower);
            }
            bombParticle.Play();
            Destroy(gameObject, 0.1f);
        }
        else
        {
            bombParticle.Play();
            Destroy(gameObject, 0.2f);
        }
    }
}
