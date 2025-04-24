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
    private Animator bearAnimation;
    private float hearingRange;

    public AudioClip notify;
    public AudioSource bearAudio;
    //particle
    public Transform particleChild;
    GameObject particleNoticeObject;
    private bool firstTime;
    public bool gotHit;


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
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Particle"))
            {
                particleNoticeObject = child.gameObject;
                break; // Found it, no need to keep checking
            }
        }
        bearAnimation = GetComponentInChildren<Animator>();
        hearingRange += detectionRange + 50;
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

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange || gotHit == true)
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
            if(firstTime == true)
            {
                firstTime = false;
            }
            bearAnimation.SetBool("isRunning", false);
            Debug.Log("Romming");
            agent.speed = roamSpeed;
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                SetNewDestination();
            }
        }
    }

    void OnGunshotHeard(Vector3 gunshotPosition)
    {
        float distance = Vector3.Distance(transform.position, gunshotPosition);
        if (distance <= hearingRange && !isAttacking)
        {
            Debug.Log($"{name} heard a gunshot and is fleeing!");
            ChasePlayer();
        }
    }
    void ChasePlayer()
    {
        if (!isAttacking)
        {
            if(firstTime != true)//particle system Notice
            {
                firstTime = true;
                particleNoticeObject.SetActive(true);
                bearAudio.clip = notify;
                bearAudio.Play();
                StartCoroutine(ParticleClose());
            }
            bearAnimation.SetBool("isRunning", true);
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
        if (Time.time - lastAttackTime >= 5f)
        {
            Debug.Log("DASH ATTACK!");
            isAttacking = true;

            // Stop agent movement and start dash
            agent.ResetPath();

            StartCoroutine(DashAtPlayer());
        }
    }
    IEnumerator ParticleClose()
    {
        yield return new WaitForSeconds(2f);
        bearAudio.Stop();
        particleNoticeObject.gameObject.SetActive(false);
    }
    IEnumerator DashAtPlayer()
    {
        float dashTime = 1.5f;
        float dashSpeed = 160f; // insane speed
        float elapsed = 0f;

        // Face the player before starting the dash

        while (elapsed < dashTime)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = lookRotation;
            directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * dashSpeed * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }

        FinishAttack();
    }

    void FinishAttack()
    {
        isAttacking = false;
        lastAttackTime = Time.time;
        SetNewDestination(); // Resume roaming or chasing
    }

    void SetNewDestination()
    {
        Vector3 randomPoint = startPosition + new Vector3(Random.Range(-roamRadius, roamRadius), 0, Random.Range(-roamRadius, roamRadius));
        agent.SetDestination(randomPoint);
    }
}
