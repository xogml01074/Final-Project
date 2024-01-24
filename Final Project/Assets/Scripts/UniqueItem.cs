using System.Collections;
using UnityEngine;
using Fusion;

public class UniqueItem : NetworkBehaviour
{
    public int randomValue;
    public GameObject ba;
    public GameObject bomber;
    public GameObject pA;
    public GameObject bC;
    public GameObject rC;


    // ����ũ ������ �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        // �浹 ��밡 Bomber �ϰ��
        if (other.gameObject.name == "Bomber(Clone)")
        {
            randomValue = 1;
            bomber.GetComponent<PlayerFire>().currentCount += 6;
            bomber.GetComponent<CharacterMovement>().currentHP = 100;
            print("Bonb Count Added");

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
        else if (other.gameObject.name == "Player_Attacker(Clone)")
        {
            randomValue = 2;
            pA.GetComponent<PlayerController>().bulletMagarzion += 30;
            pA.GetComponent<PlayerController>().hp = 100;
            print("Bullet Count Added");
            if (randomValue == Random.Range(0, 8))
            {
                bC.GetComponent<BulletCtrl>().bulletPower += 2;
                print("Bullet Power Enhanced");
            }
            else if (randomValue == Random.Range(0, 20))
            {
                rC.GetComponent<RocketController>().rocketRadius += 5f;
                rC.GetComponent<RocketController>().rocketPower += 10F;
                print("Rocket Power, Radius Enhanced");
            }
        }
        // �ƴѰ�� (�� / �� ��)
        else
        {
            print("Enemy Collected");
            destroyDelay();
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
