using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // 이펙트 소리 등등 넣을것
    // 수류탄 데미지
    public int attackPower = 20;

    // 폭발 효과 반경
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
    // 충돌했을 때의 처리
    private void OnCollisionEnter(Collision collision)
    {
        int layerMask = (1 << enemyLayer);

        // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.
        // 여기 안됨
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].gameObject.GetComponent<EnemyTest>().HitEnemy(attackPower);
            cols[i].gameObject.GetComponent<TitanScript>().HitTitan(attackPower);
        }

        bombParticle.Play();
        Destroy(gameObject, 0.1f);
    }
}
