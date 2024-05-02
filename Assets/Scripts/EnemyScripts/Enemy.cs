using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public float damageCooldown;

    public void Update()
    {
        damageCooldown = Mathf.Clamp(damageCooldown - Time.deltaTime, 0, math.INFINITY);

        if (health <= 0) Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if(damageCooldown > 0) return;
        health -= damage;
        damageCooldown = 0.01f;
    }
}
