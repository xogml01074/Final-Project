using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : NetworkBehaviour
{
    public float speed = 100f;

    public GameObject bulletHole;

    public GameObject firePoint;

    public GameObject bloodEff;

    public float bulletPower = 3f;

    Rigidbody rb;

    public Transform target;

    private NetworkRunner runner;

    public override void Spawned()
    {
        transform.forward = Camera.main.transform.forward;

        runner = GameObject.Find("NetworkCallback").GetComponent<NetworkRunner>();
        rb = GetComponent<Rigidbody>();
            
        rb.AddForce(Camera.main.transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, 5f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitPoint;


        // 접촉한 오브젝트의 레이어가 'Enemy'일 경우 피흘리는 이펙트를 생성한다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (Physics.Raycast(ray, out hitPoint))
            {
                bloodEff.transform.position = gameObject.transform.position;
                
                bloodEff.transform.forward = hitPoint.normal;

                if (collision.gameObject.tag == "Enemy")
                    collision.gameObject.GetComponent<ZombieMovement>().Hurt(bulletPower);
                else if (collision.gameObject.tag == "Boss")
                    collision.gameObject.GetComponent<TitanScript>().HitEnemy(bulletPower);

                runner.Spawn(bloodEff);

                runner.Despawn(Object);
            }
        }
        // 아닐경우 탄흔 오브젝트를 생성한다.
        else
        {
            if (Physics.Raycast(ray, out hitPoint))
            {
                bulletHole.transform.position = gameObject.transform.position;

                bulletHole.transform.forward = hitPoint.normal;
                
                runner.Spawn(bulletHole);

                runner.Despawn(Object);
            }
        }
    }

    // Unique Item을 위한 스크립트
    public void UBP(int value)
    {
        bulletPower += value;
    }
}
