using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform cameraTransform;  // Reference to the player's camera
    public Transform gunStartPoint;    // Where the gun is located
    public int gunRange = 100;         // Range of the gun
    public int bodyDamage = 1;
    public int headDamage = 2;
    public LineRenderer shootLine;     // LineRenderer component to show the shot
    public CameraSwitch cameraSwitchScript;

    private Shotgun shotgun;


    void Start () {
        shotgun = GetComponent<Shotgun>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Example: Left mouse click to shoot
        {
            ShootRaycast();
        }
    }

    public void ShootRaycast()
    {
        RaycastHit hit;

        // Start the ray from the camera and shoot forward
        Vector3 shootDirection = cameraTransform.forward;


        // Check if you're in first or third person view

        if(shotgun.isSniper == true) {
            // Perform the raycast
            if (Physics.Raycast(gunStartPoint.position, shootDirection, out hit, gunRange, enemyLayer))
            {
                Debug.Log("Hit " + hit.collider.tag);

                // Show the line from the gun to the hit point
                shootLine.SetPosition(0, gunStartPoint.position);
                shootLine.SetPosition(1, hit.point); // Set the end point at the hit location

                if (hit.collider.tag == "Enemy")
                {
                    HitBox hitboxEnemy = hit.collider.gameObject.GetComponentInParent<HitBox>();
                    hitboxEnemy.TakeDamage(bodyDamage);
                }
                else if (hit.collider.tag == "EnemyHead")
                {
                    HitBox hitboxEnemy = hit.collider.gameObject.GetComponentInParent<HitBox>();
                    hitboxEnemy.TakeDamage(headDamage);
                }

            }
            else
            {
                // If the ray doesn't hit anything, set the line to end at max range
                shootLine.SetPosition(0, gunStartPoint.position);
                shootLine.SetPosition(1, gunStartPoint.position + shootDirection * gunRange);
            }

            // Activate the line for the shot duration
            shootLine.enabled = true;

            // Optionally, hide the line after a brief time (e.g., 0.1 seconds)
            StartCoroutine(HideShootLine());

        }
        else if (shotgun.isSniper == false) {
            
            if (Physics.Raycast(cameraTransform.position, shootDirection, out hit, gunRange, enemyLayer))
            {
                
                Debug.Log("Hit " + hit.collider.tag);

                // Show the line from the gun to the hit point
                shootLine.SetPosition(0, gunStartPoint.position);
                shootLine.SetPosition(1, hit.point); // Set the end point at the hit location


            }
            else
            {
                // If the ray doesn't hit anything, set the line to end at max range
                shootLine.SetPosition(0, cameraTransform.position);
                shootLine.SetPosition(1, cameraTransform.position + shootDirection * gunRange);
            }

            // Activate the line for the shot duration
            shootLine.enabled = true;

            // Optionally, hide the line after a brief time (e.g., 0.1 seconds)
            StartCoroutine(HideShootLine());
        }

        // // Perform the raycast
        // if (Physics.Raycast(gunStartPoint.position, shootDirection, out hit, gunRange, enemyLayer))
        // {
        //     Debug.Log("Hit " + hit.collider.tag);

        //     // Show the line from the gun to the hit point
        //     shootLine.SetPosition(0, gunStartPoint.position);
        //     shootLine.SetPosition(1, hit.point); // Set the end point at the hit location

        //     // Optionally, you could add effects, like applying damage to the enemy
        //     // Example: hit.collider.GetComponent<Enemy>().TakeDamage(damage);

        // }
        // else
        // {
        //     // If the ray doesn't hit anything, set the line to end at max range
        //     shootLine.SetPosition(0, gunStartPoint.position);
        //     shootLine.SetPosition(1, gunStartPoint.position + shootDirection * gunRange);
        // }

        // // Activate the line for the shot duration
        // shootLine.enabled = true;

        // // Optionally, hide the line after a brief time (e.g., 0.1 seconds)
        // StartCoroutine(HideShootLine());
    }

    private IEnumerator HideShootLine()//hide after afew sec
    {
        // Wait for 0.1 seconds before disabling the LineRenderer
        yield return new WaitForSeconds(0.5f);
        shootLine.enabled = false;
    }
}
