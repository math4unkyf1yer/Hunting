using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerAi : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;
    public float roamRadius = 20f;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float roamSpeed = 3f;
    public float chaseSpeed = 6f;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private Vector3 startPosition;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player object has the 'Player' tag.");
        }

        startPosition = transform.position;
        agent.speed = roamSpeed;
        SetNewDestination();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
          //  AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
           
            ChasePlayer();
        }
        else
        {
            Roam();
        }
    }

    void Roam()
    {
        if (!isAttacking)
        {
            Debug.Log("Romming");
            agent.speed = roamSpeed;
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                SetNewDestination();
            }
        }
    }

    void ChasePlayer()
    {
        if (!isAttacking)
        {
            Debug.Log("chasePlayer");
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("attacking");
            isAttacking = true;
            lastAttackTime = Time.time;

            // Stop the bear's movement
            agent.ResetPath();

            // Face the player directly
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Slow the player
        /*    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SlowDown(); // Slow down the player for 1 second
                playerMovement.SetAttacking(true); // Stop player's movement during attack
            }*/

            Invoke("FinishAttack", attackCooldown); // Allow attack to complete after cooldown
        }
    }

    void FinishAttack()
    {
        isAttacking = false; // Allow bear to resume roaming or chasing

        // Resume player's movement after attack
      /*  PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetAttacking(false); // Allow player movement after attack
        }*/

        // Bear can now resume movement
        SetNewDestination();
    }

    void SetNewDestination()
    {
        Vector3 randomPoint = startPosition + new Vector3(Random.Range(-roamRadius, roamRadius), 0, Random.Range(-roamRadius, roamRadius));
        agent.SetDestination(randomPoint);
    }
}
