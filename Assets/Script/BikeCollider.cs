using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeCollider : MonoBehaviour
{
    public BikeController bikeControllerScript;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            bikeControllerScript.CheckCollision();
        }
    }
}
