using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeCollider : MonoBehaviour
{
    public BikeController bikeControllerScript;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log(collision.gameObject.tag);
            bikeControllerScript.CheckCollision();
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            bikeControllerScript.FlyCrash();
        }
    }
}
