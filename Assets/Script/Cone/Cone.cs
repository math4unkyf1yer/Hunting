using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 force = collision.relativeVelocity * 50f;
            rb.AddForce(force);
        }
    }
}
