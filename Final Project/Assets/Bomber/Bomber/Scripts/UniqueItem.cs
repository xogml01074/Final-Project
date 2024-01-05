using System.Collections;
using UnityEngine;

public class UniqueItem : MonoBehaviour
{
    public int randomValue;
    GameObject pf;
    public GameObject ba;

    private void Start()
    {
        pf = GameObject.FindWithTag("Player");
    }

    // 유니크 아이템 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        // 충돌 상대가 Player일경우
        if (other.gameObject.CompareTag("Player"))
        {
            randomValue = 1;
            pf.GetComponent<PlayerFire>().currentCount += 6;
            print("Bullet Count Added");

            if (randomValue == Random.Range(0, 8))
            {
                ba.GetComponent<BombAction>().attackPower += 10;
                print("Bomb Power Enhanced");
            }
            else if (randomValue == Random.Range(0, 20))
            {
                ba.GetComponent<BombAction>().explosionRadius += 3f;
                print("Bomb Radius Increased");
            }
            Destroy(gameObject);
        }
        // 아닌경우 (땅 / 적 등)
        else
        {
            destroyDelay();
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
