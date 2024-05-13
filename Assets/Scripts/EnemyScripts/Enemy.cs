using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth;

    public float damageCooldown;
    
    public Animator enemyDeath;

    private Rigidbody rb;

    public void Update()
    {
        damageCooldown = Mathf.Clamp(damageCooldown - Time.deltaTime, 0, math.INFINITY);

        if (enemyHealth <= 0)
        {
            //rb = GetComponent<Rigidbody>();
            //rb.freezeRotation = false;
            enemyDeath.Play("enemyDeath");
            Destroy(gameObject, 1f);
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