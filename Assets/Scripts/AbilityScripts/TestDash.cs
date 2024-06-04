using System;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TestDash : MonoBehaviour
{

    [Header("States")]
    public bool holdingShift;
    public bool chargeDone = false;
    public bool Dashing;
    public bool enemyInCircle;

    [Header("Properties")]
    [Range(0, 1.5f)]public float DashCharge;
    [Range(0, 1.5f)]public float DashCooldown;
    public float dashForce;

    [Header("References")]
    private PlayerMovement playerMovement;
    private PostProcessingManager ppm;
    public gunScript gs;
    private Rigidbody rb;
    private PStypes abilityPStypes;
    public GameObject dashFX;

    [Header("Audio")]
    public AudioSource chargeDashFire;
    public AudioSource startCharge;
    public AudioSource chargePowerUp;
    public AudioSource chargeExplosion;
    public AudioSource powerDown;
    public AudioSource chargeDoneSFX;
    public AudioSource dashWind;

    void Awake()
    {
      playerMovement = GetComponent<PlayerMovement>();
      rb = GetComponent<Rigidbody>();
      ppm = FindAnyObjectByType<PostProcessingManager>();
      abilityPStypes = FindAnyObjectByType<PStypes>();
    }


    void Update()
    {
        if(holdingShift && DashCooldown == 0)DashCharge += Time.deltaTime;
        else DashCharge -= Time.deltaTime;
        
        DashCharge = Math.Clamp (DashCharge, 0, 1.5f);

        if (DashCharge == 1.5f && DashCooldown == 0 && !chargeDone)
        {
          chargeDone = true;
          chargeDoneSFX.Play();
        }

        if(DashCooldown > 0) DashCooldown -= Time.deltaTime;
        if (DashCooldown < 0) DashCooldown = 0;
    }

    public void OnCtrl(InputAction.CallbackContext context)
    {
      if(context.started && !Dashing && DashCooldown <= 0 && playerMovement.hasDash)
      {
        holdingShift = true;
        //audio
        chargeDashFire.Play();
        chargePowerUp.Play();
        powerDown.Stop();
        //global volume
        ppm.targetVignette = 0.2f;
        ppm.targetLensDistortion = -0.7f;
      }
      else if(context.canceled && playerMovement.hasDash)
      {
        holdingShift = false;
        //audio
        chargeDashFire.Stop();
        chargePowerUp.Stop();
        if(!Dashing)powerDown.Play();
        //global volume
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

        if (DashCharge >= 1.5f)
        {
          Dash();
          DashCooldown = 1.5f;
        }
        else DashCharge = 0;
      }
    }

    private void Dash()
    {
        Dashing = true;
        startCharge.Play();
        dashWind.Play();
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

        playerMovement.Movement = playerMovement.CamF;
        playerMovement.rb.AddForce(playerMovement.Movement * dashForce, ForceMode.VelocityChange);

        //friction stop
        GetComponent<Collider>().material.dynamicFriction = 0;
        GetComponent<Collider>().material.staticFriction = 0;
        playerMovement.MaxSpeed = 30;
    }

    public virtual void OnCollisionEnter (Collision collision)
    {
      
      if (Dashing)
      {
        if(Math.Abs(collision.contacts[0].normal.x) > 0.5f || Math.Abs(collision.contacts[0].normal.z) > 0.5f)
        {
          ResetDash();
            chargeExplosion.Play();
            chargeDashFire.Stop();
            dashWind.Stop();
        }
      }
    }
    
    private void ResetDash()
    {
      Dashing = false;
      chargeDone = false;
      GetComponent<Collider>().material.dynamicFriction = 0.75f;
      GetComponent<Collider>().material.staticFriction = 0.75f;
      playerMovement.MaxSpeed = 10;

      Collider[] colliders = Physics.OverlapSphere(transform.position - playerMovement.CamF * 2, 4);
      DebugPlus.DrawWireSphere(transform.position - playerMovement.CamF * 2, 4).Duration(1);

      GameObject particle = Instantiate(dashFX, transform.position, Quaternion.identity);

      foreach(Collider collider in colliders)
      { 
        Enemy enemy = collider.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
          enemy.GetComponent<Rigidbody>().AddForce((enemy.transform.position - playerMovement.transform.position).normalized * 0.1f + Vector3.up * 0.2f, ForceMode.VelocityChange);
          enemy.TakeDamage(60);
        }
      }
    }
}
