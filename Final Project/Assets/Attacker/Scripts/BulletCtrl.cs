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

    public float bulletPower = 10f;

    public override void Spawned()
    {
        // ��� ����Ʈ ������Ʈ�� ã�´�. 
        firePoint = GameObject.Find("MuzzlePoint");

        transform.position = firePoint.transform.position;

        transform.forward = Camera.main.transform.forward;

        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed);

        StartCoroutine(DestroyDeley());
    }

    IEnumerator DestroyDeley()
    {
        yield return new WaitForSeconds(5f);

        Runner.Despawn(Object);
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
