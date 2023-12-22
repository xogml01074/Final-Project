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

    // ����ũ ������ �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        // �浹 ��밡 Player�ϰ��
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
        // �ƴѰ�� (�� / �� ��)
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
