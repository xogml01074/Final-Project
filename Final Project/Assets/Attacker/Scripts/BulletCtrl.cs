using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 100f;

    public GameObject bulletHole;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * speed);

        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                Destroy(gameObject);
                
                bulletHole.transform.position = gameObject.transform.position;

                bulletHole.transform.forward = hitPoint.normal;

                Instantiate(bulletHole);
            }
        }
    }
}
