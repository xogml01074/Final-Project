using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MineScript : NetworkBehaviour
{
    public int mPower = 100;
    public float explosionRadius = 30f;
    public ParticleSystem mineParticle;
    public GameObject mine;
    public LayerMask enemyLayer;
    public bool isPlanted;

    public override void Spawned()
    {
        mineParticle = GetComponentInChildren<ParticleSystem>();
        mineParticle.Pause();
        isPlanted = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlanted = true;
        // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

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
