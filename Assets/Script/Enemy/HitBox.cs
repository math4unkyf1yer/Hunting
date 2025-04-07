using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int health;
    public int refillAmount;
    private Shoot shootScript;

    private void Start()
    {
        GameObject ak = GameObject.Find("Ak47Holder");
        shootScript = ak.gameObject.GetComponent<Shoot>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        // refill bullets
        shootScript.amountOfBullet += refillAmount;
        Destroy(gameObject);
    }
}
