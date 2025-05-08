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

    public GameObject body;
    public Material shinyGold;

    public AudioClip notify;
    public AudioSource deerAudio;

    //Particle Effects
    [SerializeField] ParticleSystem _dustPart;
    public Transform particleChild;
    GameObject particleObject;
    private bool firstTime;


    public Animator deerAnimation;
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
                particleChild = child;
                break; // Found it, no need to keep checking
            }
        }

        if (particleChild != null)
        {
            particleObject = particleChild.gameObject;
            // Now you can use particleObject
        }

        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        agent.speed = patrolSpeed;
        player = GameObject.Find("BikeBodyHolder").GetComponent<Transform>();
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
        UpdateAnimationState();
    }

    void PatrolArea()
    {
        if (isRunningAway) return; // Don't patrol if fleeing

        if (firstTime == true)
        {
            firstTime = false;
        }
        Vector3 randomPoint = originalPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        agent.speed = patrolSpeed;
        agent.SetDestination(randomPoint);

        _dustPart.Stop();
    }

    void RunAwayFrom(Vector3 threatPosition)
    {
        if (firstTime != true)//particle system Notice
        {
            firstTime = true;
            particleObject.SetActive(true);
            deerAudio.clip = notify;
            deerAudio.Play();
            StartCoroutine(ParticleClose());
        }
        Vector3 directionAway = (transform.position - threatPosition).normalized;
        Vector3 runToPosition = transform.position + directionAway * runDistance;

        agent.speed = chasespeed;
        agent.SetDestination(runToPosition);
        isRunningAway = true;

        _dustPart.Play();
    }

    void OnGunshotHeard(Vector3 gunshotPosition)
    {
        float distance = Vector3.Distance(transform.position, gunshotPosition);
        if (distance <= hearingRange && !isRunningAway)
        {
            RunAwayFrom(gunshotPosition);
        }
    }


    IEnumerator ParticleClose()
    {
        yield return new WaitForSeconds(2f);
        deerAudio.Stop();
        particleObject.gameObject.SetActive(false);
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
    void UpdateAnimationState()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            if (isRunningAway)
            {
                deerAnimation.SetBool("Running", true);
                deerAnimation.SetBool("Walking", false);
            }
            else
            {
                deerAnimation.SetBool("Running", false);
                deerAnimation.SetBool("Walking", true);
            }
        }
        else
        {
            // Not moving
            deerAnimation.SetBool("Running", false);
            deerAnimation.SetBool("Walking", false);
        }
    }
}

