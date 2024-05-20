using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3;
    public bool testDashOneTime = false;
    private gunScript gs;
    private PlayerMovement pm;
    private DestroyText dt;
    private TestDash td;
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
                    //Dash True
                    gs.GetComponent<TestDash>().enabled = true;
                    pm.hasDash = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    gs.GetComponent<RadioActiveNuke>().enabled = false;
                    pm.hasRadioactiveNuke = false;

                    gs.GetComponent<LightningStrike>().enabled = false;
                    pm.hasLightningStrike = false;

                    gs.GetComponent<Teleportation>().enabled = false;
                    pm.hasTeleport = false;
                    
                    gs.GetComponent<GroundPound>().enabled = false;
                    pm.hasGroundPound = false;

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                }
                else if (OnShoot.abilityType.ToLower() == "nuke" && !pm.hasRadioactiveNuke)
                {
                    //Nuke True
                    gs.GetComponent<TestDash>().enabled = false;
                    pm.hasDash = false;

                    gs.GetComponent<RadioActiveNuke>().enabled = true;
                    pm.hasRadioactiveNuke = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    gs.GetComponent<LightningStrike>().enabled = false;
                    pm.hasLightningStrike = false;

                    gs.GetComponent<Teleportation>().enabled = false;
                    pm.hasTeleport = false;

                    gs.GetComponent<GroundPound>().enabled = false;
                    pm.hasGroundPound = false;

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "lightning" && !pm.hasLightningStrike)
                {
                    //Lightning True
                    gs.GetComponent<TestDash>().enabled = false;
                    pm.hasDash = false;

                    gs.GetComponent<RadioActiveNuke>().enabled = false;
                    pm.hasRadioactiveNuke = false;

                    gs.GetComponent<LightningStrike>().enabled = true;
                    pm.hasLightningStrike = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    gs.GetComponent<Teleportation>().enabled = false;
                    pm.hasTeleport = false;

                    gs.GetComponent<GroundPound>().enabled = false;
                    pm.hasGroundPound = false;

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "teleport" && !pm.hasTeleport)
                {
                    //Teleport True
                    gs.GetComponent<TestDash>().enabled = false;
                    pm.hasDash = false;

                    gs.GetComponent<RadioActiveNuke>().enabled = false;
                    pm.hasRadioactiveNuke = false;

                    gs.GetComponent<LightningStrike>().enabled = false;
                    pm.hasLightningStrike = false;

                    gs.GetComponent<Teleportation>().enabled = true;
                    pm.hasTeleport = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    gs.GetComponent<GroundPound>().enabled = false;
                    pm.hasGroundPound = false;

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
                else if (OnShoot.abilityType.ToLower() == "groundpound" && !pm.hasGroundPound)
                {
                    //Ground Pound True
                    gs.GetComponent<TestDash>().enabled = false;
                    pm.hasDash = false;

                    gs.GetComponent<RadioActiveNuke>().enabled = false;
                    pm.hasRadioactiveNuke = false;

                    gs.GetComponent<LightningStrike>().enabled = false;
                    pm.hasLightningStrike = false;

                    gs.GetComponent<Teleportation>().enabled = false;
                    pm.hasTeleport = false;

                    gs.GetComponent<GroundPound>().enabled = true;
                    pm.hasGroundPound = true;
                    OnShoot.text.SetActive(true);
                    td.chargeDoneSFX.Play();

                    ParticleSystem part = OnShoot.GetComponentInChildren<ParticleSystem>();
                    part.Play();
                    Destroy(gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
