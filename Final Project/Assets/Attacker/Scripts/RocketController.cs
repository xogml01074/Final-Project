using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : NetworkBehaviour
{
    public float rt;

    public GameObject target;

    public GameObject eff_explosion;

    public float speed = 100f;

    public GameObject firePoint;

    public float rocketPower = 40f;

    public float rocketRadius = 15f;

    public override void Spawned()
    {
        transform.forward = Camera.main.transform.forward;

        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, rocketRadius, 1 << 8);
        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i].gameObject.tag == "Boss")
                coll[i].GetComponent<TitanScript>().HitEnemy((int)rocketPower);
            else if (coll[i].gameObject.tag == "Enemy")
            {
                if (Vector3.Distance(coll[i].transform.position, transform.position) > 12)
                    coll[i].GetComponent<ZombieMovement>().Hurt(10);
                else if (Vector3.Distance(coll[i].transform.position, transform.position) > 9)
                    coll[i].GetComponent<ZombieMovement>().Hurt(20);
                else if (Vector3.Distance(coll[i].transform.position, transform.position) > 6)
                    coll[i].GetComponent<ZombieMovement>().Hurt(30);
                else
                    coll[i].GetComponent<ZombieMovement>().Hurt(rocketPower);
            }
        }

        eff_explosion.transform.position = gameObject.transform.position;

        Runner.Spawn(eff_explosion);

        Runner.Despawn(Object);
    }

    // Unique Item 을 위한 스크립트
    public void URV(float value)
    {
        rocketRadius += value;
        rocketPower += value;
    }
}

