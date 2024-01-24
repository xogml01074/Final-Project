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
            // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

            // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.
            // 여기 안됨
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
