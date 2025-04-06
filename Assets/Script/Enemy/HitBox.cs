using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health == 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        Destroy(gameObject);
    }
}
