using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3;
    private Enemy enemy;

    void Awake()
    {
        enemy = FindAnyObjectByType<Enemy>();
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            enemy.TakeDamage(10);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
