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
    public float explosionRadius = 7f;

    public ParticleSystem bombParticle;

    public GameObject bomb;

    private void Start()
    {
        bombParticle = GetComponentInChildren<ParticleSystem>();
        bombParticle.Pause();
    }
    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ExplosionDelay());
    }

    IEnumerator ExplosionDelay()
    {
        // 3�� ��ٸ�
        yield return new WaitForSeconds(3f);

        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere (transform.position, explosionRadius, 1 << 8);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].gameObject.GetComponent<EnemyTest>().HitEnemy(attackPower);
        }

        bombParticle.Play();
        Destroy(gameObject, 0.1f);
    }
}
