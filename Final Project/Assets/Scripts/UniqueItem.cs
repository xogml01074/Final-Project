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
        print("����");
        // �浹 ��밡 Bomber �ϰ��
        if (other.gameObject.name == "Bomber(Clone)")
        {
            randomValue = 1;
            other.GetComponent<PlayerFire>().UBC(6);
            other.GetComponent<CharacterMovement>().Heal(100);
            print("Bomb Count Added");

            if (randomValue == Random.Range(0, 8))
            {
                ba.GetComponent<BombAction>().EAP(10);
                print("Bomb Power Enhanced");
            }
            else if (randomValue == Random.Range(0, 20))
            {
                ba.GetComponent<BombAction>().EER(3f);
                print("Bomb Radius Increased");
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.name == "Player_Attacker(Clone)")
        {
            randomValue = 2;
            other.GetComponent<PlayerController>().BC(30);
            other.GetComponent<PlayerController>().Heal(100);
            print("Bullet Count Added");
            if (randomValue == Random.Range(0, 8))
            {
                bC.GetComponent<BulletCtrl>().UBP(2);
                print("Bullet Power Enhanced");
            }
            else if (randomValue == Random.Range(0, 20))
            {
                rC.GetComponent<RocketController>().URV(10f);
                print("Rocket Power, Radius Enhanced");
            }
        }
        // �ƴѰ��
        else
        {
            print("Unique Item Dropped!");
        }
    }
}
