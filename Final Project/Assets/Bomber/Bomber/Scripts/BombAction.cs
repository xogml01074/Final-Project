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
    public float explosionRadius = 999999f;

    public ParticleSystem bombParticle;

    public GameObject bomb;

    LayerMask enemyLayer;

    private void Start()
    {
        bombParticle = GetComponentInChildren<ParticleSystem>();
        bombParticle.Pause();

        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        int layerMask = (1 << enemyLayer);

        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
        // ���� �ȵ�
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].gameObject.GetComponent<EnemyTest>().HitEnemy(attackPower);
            cols[i].gameObject.GetComponent<TitanScript>().HitTitan(attackPower);
        }

        bombParticle.Play();
        Destroy(gameObject, 0.1f);
    }
}
