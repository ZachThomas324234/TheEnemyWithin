using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class bullet : MonoBehaviour
{
    public float life = 3;
    public bool bossDead = true;
    private gunScript gs;
    private PlayerMovement pm;
    private DestroyText dt;
    private TestDash td;
    private PostProcessingManager ppm;
    private RadioActiveNuke rn;
    private LightningStrike ls;
    private Teleportation tele;
    private GroundPound gp;
    public ParticleSystem ps;
    public SpriteRenderer sr;

    void Awake()
    {
        Destroy(gameObject, life);
        gs = FindAnyObjectByType<gunScript>();
        pm = FindAnyObjectByType<PlayerMovement>();
        dt = FindAnyObjectByType<DestroyText>();
        td = FindAnyObjectByType<TestDash>();
        rn = FindAnyObjectByType<RadioActiveNuke>();
        ls = FindAnyObjectByType<LightningStrike>();
        tele = FindAnyObjectByType<Teleportation>();
        gp = FindAnyObjectByType<GroundPound>();
        ppm = FindAnyObjectByType<PostProcessingManager>();
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
                    DisableAllAbilities();
                    
                    ppm.vignette.color.value = OnShoot.color;

                    //Dash True
                    gs.GetComponent<TestDash>().enabled = true;
                    pm.hasDash = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                }
                else if (OnShoot.abilityType.ToLower() == "nuke" && !pm.hasRadioactiveNuke)
                {
                    DisableAllAbilities();

                    ppm.vignette.color.value = OnShoot.color;

                    gs.GetComponent<RadioActiveNuke>().enabled = true;
                    pm.hasRadioactiveNuke = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "lightning" && !pm.hasLightningStrike)
                {
                    DisableAllAbilities();

                    ppm.vignette.color.value = OnShoot.color;

                    gs.GetComponent<LightningStrike>().enabled = true;
                    pm.hasLightningStrike = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "teleport" && !pm.hasTeleport)
                {
                    DisableAllAbilities();

                    ppm.vignette.color.value = OnShoot.color;   

                    gs.GetComponent<Teleportation>().enabled = true;
                    pm.hasTeleport = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "groundpound" && !pm.hasGroundPound)
                {
                    //Ground Pound True
                    DisableAllAbilities();

                    ppm.vignette.color.value = OnShoot.color;

                    gs.GetComponent<GroundPound>().enabled = true;
                    pm.hasGroundPound = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "exitscene" && bossDead)
                {
                    Application.Quit();
                }
            }
            Destroy(gameObject);
        }
    }

    public void DisableAllAbilities()
    {
        ppm.targetLensDistortion = 0;
        ppm.targetVignette = 0;

        gs.GetComponent<TestDash>().enabled = false;
        pm.hasDash = false;

        gs.GetComponent<RadioActiveNuke>().enabled = false;
        pm.hasRadioactiveNuke = false;

        gs.GetComponent<LightningStrike>().enabled = false;
        pm.hasLightningStrike = false;

        gs.GetComponent<Teleportation>().enabled = false;
        pm.hasTeleport = false;

        gs.GetComponent<GroundPound>().enabled = false;
        pm.hasGroundPound = false;
    }
}
