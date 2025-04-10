using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float HP;
    public void RemoveHP(float hp, int minusPoints) {
        HP -=  minusPoints;

        if(HP <= 0) {
            Death();
        }
    }


    public float GetHP() {
        return this.HP;
    }

    private void Death() {

        Destroy(this.gameObject);
    }
}
