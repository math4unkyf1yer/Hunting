using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int health;
    public int refillAmount;
    public bool isAdeer;
    public bool isABird;
    public bool isABear;
    private Shoot shootScript;
    private SpawnScript spawnScript;
    public AudioSource deerSource;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public GameObject Enemy;
    private Collider enemyCollider;
    public Material oldMaterial;
    public Material newMaterial;
    private Renderer enemyRenderer;

    //particle
    private GameObject deadParticle;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("DeadParticle"))
            {
                deadParticle = child.gameObject;
                break; // Found it, no need to keep checking
            }
        }
        enemyCollider = Enemy.GetComponent<Collider>();
        enemyRenderer = Enemy.GetComponent<Renderer>();
        if(enemyRenderer == null)
        {
            enemyRenderer = Enemy.GetComponentInChildren<Renderer>();
        }
        GameObject ak = GameObject.Find("Ak47Holder");
        shootScript = ak.gameObject.GetComponent<Shoot>();
        spawnScript = GameObject.Find("GameManager").GetComponent<SpawnScript>();
        deerSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0)
        {
            deerSource.clip = hurtAudio;
            deerSource.pitch = UnityEngine.Random.Range(0.8f, 1.3f);
            deerSource.Play();
            enemyRenderer.material = newMaterial;
            StartCoroutine(ChangeMaterialGotHurt());
        }

        if (health <= 0)
        {
            deerSource.clip = deathAudio;
            deerSource.pitch = UnityEngine.Random.Range(0.8f, 1.3f);

            StartCoroutine(Dead());
        }
    }

    IEnumerator ChangeMaterialGotHurt()
    {
        yield return new WaitForSeconds(.5f);
        enemyRenderer.material = oldMaterial;
    }

    IEnumerator Dead()
    {
        if (!enemyCollider)
        {
            enemyCollider.enabled = false;
        }
        
        enemyRenderer.gameObject.SetActive(false);
        deadParticle.gameObject.SetActive(true);
        deerSource.clip = deathAudio;
        deerSource.Play();
        yield return new WaitForSeconds(1f);
        // refill bullets
        shootScript.IncreaseAmmo(refillAmount);
        spawnScript.isAlive -= 1;
        spawnScript.isABird = isABird;
        spawnScript.isAdeer = isAdeer;
        spawnScript.isABear = isABear;
        Destroy(Enemy);
    }
}
