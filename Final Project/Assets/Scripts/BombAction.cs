using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombAction : NetworkBehaviour
{
    // 이펙트 소리 등등 넣을것
    // 수류탄 데미지
    public int attackPower = 20;

    // 폭발 효과 반경
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

    // 충돌했을 때의 처리
    private void OnCollisionEnter(Collision collision)
    {
<<<<<<< Updated upstream
        if (collision.gameObject.name == "Zombie1(Clone)" || collision.gameObject.name == "Zombie2(Clone)")
        {
            Debug.Log("Zombie Hit");
            // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

            // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.

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
        // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);

        bombParticle.Play();
        // 저장된 Collider 배열에 있는 모든 에너미에게 수류탄 데미지를 적용한다.
        // 여기 안됨
        for (int i = 0; i < cols.Length; ++i)
        {
            cols[i].GetComponent<ZombieMovement>().Hurt(attackPower);
            // cols[i].GetComponent<TitanScript>().HitEnemy(attackPower);
        }
        Destroy(gameObject, 0.1f);
>>>>>>> Stashed changes
    }
}
