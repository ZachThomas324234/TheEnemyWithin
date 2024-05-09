using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth;

    public float damageCooldown;
    
    public Animator enemyDeath;

    public void Update()
    {
        damageCooldown = Mathf.Clamp(damageCooldown - Time.deltaTime, 0, math.INFINITY);

        if (enemyHealth <= 0)
        {
            enemyDeath.Play("enemyDeath");
            //Die();
        }
        // Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if(damageCooldown > 0) return;
        enemyHealth -= damage;
        damageCooldown = 0.01f;
    }
}