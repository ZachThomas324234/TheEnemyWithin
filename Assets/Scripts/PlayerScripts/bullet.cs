using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3;
    private gunScript gs;
    private PlayerMovement pm;
    private TestDash td;
    private GroundPound gp;
    public ParticleSystem ps;
    public SpriteRenderer sr;
    //public TextMeshProUGUI groundPoundText;

    void Awake()
    {
        Destroy(gameObject, life);
        gs = FindAnyObjectByType<gunScript>();
        pm = FindAnyObjectByType<PlayerMovement>();
        td = FindAnyObjectByType<TestDash>();
        gp = FindAnyObjectByType<GroundPound>();
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
                if (OnShoot.abilityType.ToLower() == "dash" && !pm.hasDash && !pm.hasGroundPound)
                {
                    gs.GetComponent<TestDash>().enabled = true;
                    pm.hasDash = true;
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                }
                else if (OnShoot.abilityType.ToLower() == "groundpound" && !pm.hasDash && !pm.hasGroundPound)
                {
                    gs.GetComponent<GroundPound>().enabled = true;
                    pm.hasGroundPound = true;
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                }
            }
            Destroy(gameObject);
        }
    }
}
