using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ����Ʈ �Ҹ� ��� ������
    // ����ź ������
    public int attackPower = 20;

    // ���� ȿ�� �ݰ�
    public float explosionRadius = 15f;

    public ParticleSystem bombParticle;

    public GameObject bomb;

    public LayerMask enemyLayer;

    private void Start()
    {
        bombParticle = GetComponentInChildren<ParticleSystem>();
        bombParticle.Pause();
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
            cols[i].GetComponent<ZombieMovement>().Hurt(attackPower);
            cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
        }

        bombParticle.Play();
        Destroy(gameObject, 0.1f);
    }
}
