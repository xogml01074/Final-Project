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
<<<<<<< Updated upstream
        bombParticle = transform.GetChild(0).GetChild(11).GetComponent<ParticleSystem>();
=======
        bombParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
>>>>>>> Stashed changes
    }

    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
<<<<<<< Updated upstream
        if (collision.gameObject.name == "Zombie1(Clone)" || collision.gameObject.name == "Zombie2(Clone)")
        {
            Debug.Log("Zombie Hit");
            // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

            // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.

            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i].GetComponent<ZombieMovement>().Hurt(attackPower);
            }
            bombParticle.Play();
            Destroy(gameObject, 0.5f);
        }
        else if (collision.gameObject.name == "Titan(Clone)")
        {
            Debug.Log("Titan Hit");
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
            }
            bombParticle.Play();
            Destroy(gameObject, 0.1f);
        }
        else
        {
            bombParticle.Play();
            Destroy(gameObject, 0.2f);
        }
=======
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        bombParticle.Play();
        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
        // ���� �ȵ�
        for (int i = 0; i < cols.Length; ++i)
        {
            cols[i].GetComponent<ZombieMovement>().Hurt(attackPower);
            // cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
        }
        Destroy(gameObject, 0.1f);
>>>>>>> Stashed changes
    }
}
