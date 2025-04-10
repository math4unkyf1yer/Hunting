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
    public float hearingRange = 40f;
    private NavMeshAgent agent;
    private Vector3 patrolDestination;
    private Vector3 originalPosition;
    private bool isRunningAway = false;

    public bool isShiny = false;
    public float shinyChance = 0.3f;
    public string deerType = "Normal";

    void OnEnable()
    {
        GunBroadCast.OnGunshotFired += OnGunshotHeard;
    }

    void OnDisable()
    {
        GunBroadCast.OnGunshotFired -= OnGunshotHeard;
    }
    void Start()
    {
        CheckShiny();

        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        agent.speed = patrolSpeed;

        PatrolArea();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Immediately flee if the player is in detection range
        if (distanceToPlayer < detectionRange)
        {
            if (!isRunningAway)
            {
                RunAwayFrom(player.position);
            }
        }
        // If reached run destination, go back to patrol
        else if (isRunningAway && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            isRunningAway = false;
            agent.speed = patrolSpeed;
            PatrolArea();
        }
        // Continue patrolling only if not fleeing
        else if (!isRunningAway && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PatrolArea();
        }
    }

    void PatrolArea()
    {
        if (isRunningAway) return; // Don't patrol if fleeing

        Vector3 randomPoint = originalPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        agent.speed = patrolSpeed;
        agent.SetDestination(randomPoint);
    }

    void RunAwayFrom(Vector3 threatPosition)
    {
        Vector3 directionAway = (transform.position - threatPosition).normalized;
        Vector3 runToPosition = transform.position + directionAway * runDistance;

        agent.speed = chasespeed;
        agent.SetDestination(runToPosition);
        isRunningAway = true;
    }

    void OnGunshotHeard(Vector3 gunshotPosition)
    {
        float distance = Vector3.Distance(transform.position, gunshotPosition);
        if (distance <= hearingRange && !isRunningAway)
        {
            Debug.Log($"{name} heard a gunshot and is fleeing!");
            RunAwayFrom(gunshotPosition);
        }
    }

    void CheckShiny()
    {
        if (Random.Range(0f, 1f) <= shinyChance)
        {
            isShiny = true;
            deerType = "Shiny";
            Debug.Log("A shiny deer has spawned!");
        }
    }

    void ChangeAppearance()
    {
        GetComponent<Renderer>().material.color = isShiny ? Color.green : Color.red;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isShiny ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}

