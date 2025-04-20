using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAi : MonoBehaviour
{
    public float speed = 20f;
    public Transform centerPoint;
    public Transform[] patrolPoint;
    public float radius = 10f;
    public float angle = 0f;
    public float arriveThreshold = 0.5f;
    public float rotationSpeed = 20;

    private Transform targetPoint;

    public bool circleMovement;

    void Start()
    {
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("PatrolPoint");
        patrolPoint = new Transform[pointObjects.Length];

        for (int i = 0; i < pointObjects.Length; i++)
        {
            patrolPoint[i] = pointObjects[i].transform;
        }

        if (patrolPoint.Length == 0)
        {
            Debug.LogWarning("No patrol points with tag found!");
            return;
        }

        GetNewTarget();
    }

    void Update()
    {
        if (patrolPoint == null)
        {
            return;
        }
        if (circleMovement == true)
        {
            angle += speed * Time.deltaTime;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            transform.position = new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z);
        }
        else
        {
            if (targetPoint == null || patrolPoint.Length == 0)
                return;
            Vector3 direction = (targetPoint.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // Check if we've arrived
            float dist = Vector3.Distance(transform.position, targetPoint.position);
            if (dist < arriveThreshold)
            {
                GetNewTarget();
            }
        }
    }
    void GetNewTarget()
    {
        if (patrolPoint == null || patrolPoint.Length == 0)
            return;

        // Filter out any null points to avoid issues
        List<Transform> validPoints = new List<Transform>();
        foreach (var point in patrolPoint)
        {
            if (point != null)
                validPoints.Add(point);
        }

        if (validPoints.Count == 0)
            return;

        int safety = 0;
        Transform newTarget;
        do
        {
            newTarget = validPoints[Random.Range(0, validPoints.Count)];
            safety++;
        }
        while (newTarget == targetPoint && validPoints.Count > 1 && safety < 100);

        targetPoint = newTarget;

    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint.position, radius);
    }
}
