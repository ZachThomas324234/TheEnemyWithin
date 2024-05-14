using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3;
    private gunScript gs;
    private PlayerMovement pm;
    private TestDash td;
    public ParticleSystem ps;
    public SpriteRenderer sr;

    void Awake()
    {
        Destroy(gameObject, life);
        gs = FindAnyObjectByType<gunScript>();
        pm = FindAnyObjectByType<PlayerMovement>();
        td = FindAnyObjectByType<TestDash>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponentInParent<Enemy>().TakeDamage(20);
            Destroy(gameObject);
        }
        else
        {
            onShoot OnShoot = collision.collider.GetComponent<onShoot>();
            if (OnShoot != null)
            {
                if (OnShoot.abilityType.ToLower() == "dash" && !pm.hasDash)
                {
                    gs.GetComponent<TestDash>().enabled = true;
                    pm.hasDash = true;
                    td.chargeDoneSFX.Play();

                    var em = ps.emission;
                    em.enabled = true;
                    ps.Play();
                }
            }
            Destroy(gameObject);
        }
    }
}
