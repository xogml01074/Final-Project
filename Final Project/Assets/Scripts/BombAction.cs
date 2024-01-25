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

    public override void Spawned()
    {
        bombParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    // 충돌했을 때의 처리
    private void OnCollisionEnter(Collision collision)
    {
        LayerMask eLayer = LayerMask.NameToLayer("Enemy");
        int msk = (1 << eLayer);
        if (collision.gameObject.name == "Zombie1(Clone)" || collision.gameObject.name == "Zombie2(Clone)")
        {
            // 폭발 효과 반경 내에서 레이어가 'Enemy'(8)인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, msk);

            foreach (Collider cs in cols)
            {
                if (cs.name == "Default") continue;
                print(cs.name);
                cs.gameObject.GetComponent<ZombieMovement>().Hurt(attackPower);
            }
            /*for (int i = 0; i < cols.Length; i++)
            {
                cols[i].gameObject.GetComponent<ZombieMovement>().Hurt(attackPower);
            }*/
            bombParticle.Play();
            Destroy(gameObject, 0.5f);
        }
        else if (collision.gameObject.name == "Boss(Clone)")
        {
            print("Boss Hit");
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i].gameObject.GetComponent<TitanScript>().HitEnemy(attackPower);
            }
            bombParticle.Play();
            Destroy(gameObject, 0.1f);
        }
        else
        {
            bombParticle.Play();
            Destroy(gameObject, 0.2f);
        }
    }

    public void EAP(int value)
    {
        attackPower += value;
    }

    public void EER(float value)
    {
        explosionRadius += value;
    }
}
