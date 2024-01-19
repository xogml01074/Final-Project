using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public int mPower = 50;
    public float explosionRadius = 30f;
    public ParticleSystem mineParticle;
    public GameObject mine;
    public LayerMask enemyLayer;
    public float currentTime;

    private void Start()
    {
        mineParticle = GetComponent<ParticleSystem>();
        mineParticle.Pause();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // 충돌하지 않아도 일정 시간 후에 자동으로 터짐
        if (currentTime >= 10F)
        {
            Collider[] co = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < co.Length; i++)
            {
                co[i].GetComponent<ZombieMovement>().Hurt(mPower);
            }
            mineParticle.Play();
            Destroy(gameObject, 0.2f);
        }
    }
    // 충돌했을 때의 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        print(collision);

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
