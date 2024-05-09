using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider enemyHealthSlider;
    public Slider enemyEaseHealthSlider;
    public float enemyMaxHealth = 100f;
    private Enemy enemy;
    private float enemyLerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemy.enemyHealth = enemyMaxHealth;        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealthSlider.value != enemy.enemyHealth)
        {
            enemyHealthSlider.value = enemy.enemyHealth;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            takeDamage(10);
        }

        if (enemyHealthSlider.value != enemyEaseHealthSlider.value)
        {
            enemyEaseHealthSlider.value = Mathf.Lerp(enemyEaseHealthSlider.value, enemy.enemyHealth, enemyLerpSpeed);
        }
    }

    void takeDamage(float damage)
    {
        enemy.enemyHealth -= damage;
    }
}
