using Fusion;
using UnityEngine;

public class MineScript : NetworkBehaviour
{
    public int mPower = 100;
    public float explosionRadius = 30f;
    public ParticleSystem mineParticle;
    public GameObject mine;
    public LayerMask enemyLayer;

    public override void Spawned()
    {
        mineParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Zombie1(Clone)" || other.gameObject.name == "Zombie2(Clone)")
        {
            // ���� ȿ�� �ݰ� ������ ���̾ 'Enemy'(8)�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

            // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
            // ���� �ȵ�
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].GetComponent<ZombieMovement>().Hurt(mPower);
            }
            mineParticle.Play();
            Destroy(gameObject, 0.3f);
        }
        else if (other.gameObject.name == "Titan(Clone)")
        {
            mineParticle.Play();
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].GetComponent<TitanScript>().HitEnemy(mPower);
            }
            mineParticle.Play();
            Destroy(gameObject, 0.1f);
        }
    }
}
