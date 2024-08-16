using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesShield : MonoBehaviour, IDamageable
{
    public int health;
    public bool isBroken;

    public void GetHit(float damage)
    {
        health -= 1;
        if (health <= 0)
            isBroken = true;
    }

    public void Repelled(float force)
    {
        
    }

    private void OnEnable()
    {
        isBroken = false;
        health = 7;
    }
}
