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
        // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.
        // 여기 안됨
        for (int i = 0; i < cols.Length; ++i)
        {
            cols[i].GetComponent<ZombieMovement>().Hurt(mPower);
            // cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
        }

        mineParticle.Play();
        Destroy(gameObject, 0.2f);
    }
}
