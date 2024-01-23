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

    public float bulletPower = 20f;

    Rigidbody rb;

    public Transform target;

    public override void Spawned()
    {
        transform.forward = Camera.main.transform.forward;

        rb = GetComponent<Rigidbody>();
            
        rb.AddForce(Camera.main.transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitPoint;


        // ������ ������Ʈ�� ���̾ 'Enemy'�� ��� ���긮�� ����Ʈ�� �����Ѵ�.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (Physics.Raycast(ray, out hitPoint))
            {
                bloodEff.transform.position = gameObject.transform.position;
                
                bloodEff.transform.forward = hitPoint.normal;

                collision.gameObject.GetComponent<ZombieMovement>().Hurt(bulletPower);

                Runner.Spawn(bloodEff);

                Runner.Despawn(Object);
            }
        }
        // �ƴҰ�� ź�� ������Ʈ�� �����Ѵ�.
        else
        {
            if (Physics.Raycast(ray, out hitPoint))
            {
                bulletHole.transform.position = gameObject.transform.position;

                bulletHole.transform.forward = hitPoint.normal;
                
                Runner.Spawn(bulletHole);

                Runner.Despawn(Object);
            }
        }
    }
}
