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

    private void Start()
    {
        
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
        }

        if (health <= 0)
        {
            deerSource.clip = deathAudio;
            deerSource.pitch = UnityEngine.Random.Range(0.8f, 1.3f);
            AudioSource.PlayClipAtPoint(deathAudio, transform.position, 1f);

            Dead();
        }
    }

    void Dead()
    {
        // refill bullets
        shootScript.IncreaseAmmo(refillAmount);
        spawnScript.isAlive -= 1;
        spawnScript.isABird = isABird;
        spawnScript.isAdeer = isAdeer;
        spawnScript.isABear = isABear;
        Destroy(Enemy);
    }
}
