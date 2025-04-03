using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeerAi : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float runDistance = 20f;
    public float patrolRange = 50f;
    public float patrolSpeed = 3f;
    public float chasespeed = 10f;

    private NavMeshAgent agent;
    private Vector3 patrolDestination;
    private Vector3 originalPosition;
    private bool isRunningAway = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        agent.speed = patrolSpeed;
        PatrolArea();
    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && !isRunningAway)
        {
            RunAway();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f && !isRunningAway)
        {
            isRunningAway = false;
            agent.speed = patrolSpeed;
            PatrolArea();
        }
        else if (isRunningAway && !agent.pathPending && agent.remainingDistance < 0.5f)
        {

            isRunningAway = false;
            agent.speed = patrolSpeed;
            PatrolArea();
        }
    }

    void PatrolArea()
    {
        Vector3 randomPoint = originalPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        agent.SetDestination(randomPoint);
    }

    void RunAway()
    {
        Vector3 directionAway = (transform.position - player.position).normalized;
        Vector3 newRunPosition = transform.position + directionAway * runDistance;

        agent.speed = chasespeed;
        agent.SetDestination(newRunPosition);
        isRunningAway = true;
    }
}
