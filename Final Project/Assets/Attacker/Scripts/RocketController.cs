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

    public float rocketPower = 50f;

    public float rocketRadius = 5f;

    public override void Spawned()
    {
        firePoint = GameObject.Find("MuzzlePoint");

        transform.position = firePoint.transform.position;

        transform.forward = Camera.main.transform.forward;

        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed);

        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, rocketRadius, 1 << 8);
        for (int i = 0; i < coll.Length; i++)
        {
            coll[i].GetComponent<ZombieMovement>().Hurt(rocketPower);
        }

        eff_explosion.transform.position = gameObject.transform.position;

        Runner.Spawn(eff_explosion);
        
        Runner.Despawn(Object);
    }
}
