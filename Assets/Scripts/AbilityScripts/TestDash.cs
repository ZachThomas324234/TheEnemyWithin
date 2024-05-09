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
    [Range(0, 0.5f)]public float DashDuration;
    public float dashForce;

    [Header("References")]
    private PlayerMovement playerMovement;
    private Rigidbody rb;
    private Volume volume;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    [Header("Audio")]
    public AudioSource chargeDashFire;
    public AudioSource startCharge;
    public AudioSource chargePowerUp;
    public AudioSource chargeExplosion;
    public AudioSource powerDown;
    public AudioSource chargeDoneSFX;
    public AudioSource dashWind;

    private float targetLensDistortion;
    private float blendLensDistortion;
    private float targetVignette;
    private float blendVignette;

    void Awake()
    {
      playerMovement = GetComponent<PlayerMovement>();
      rb = GetComponent<Rigidbody>();
      volume = FindAnyObjectByType<Volume>();
      volume.profile.TryGet (out Vignette v);
      vignette = v;
      volume.profile.TryGet (out LensDistortion lD);
      lensDistortion = lD;
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

        vignette.intensity.value = Mathf.SmoothDamp(vignette.intensity.value, targetVignette, ref blendVignette, 0.5f);
        lensDistortion.intensity.value = Mathf.SmoothDamp(lensDistortion.intensity.value, targetLensDistortion, ref blendLensDistortion, 0.5f);
    }

    public void OnShift(InputAction.CallbackContext context)
    {
      if(context.started && !Dashing && DashCooldown <= 0)
      {
        holdingShift = true;
        //audio
        chargeDashFire.Play();
        chargePowerUp.Play();
        powerDown.Stop();
        //global volume
        targetVignette = 0.2f;
        targetLensDistortion = -0.7f;
        
        //if (readyToGo) powerDown.Play()
      }
      else if(context.canceled)
      {
        holdingShift = false;
        //audio
        chargeDashFire.Stop();
        chargePowerUp.Stop();
        if(!Dashing)powerDown.Play();
        //global volume
        targetVignette = 0;
        targetLensDistortion = 0;

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
        targetVignette = 0;
        targetLensDistortion = 0;

        playerMovement.Movement = playerMovement.CamF;
        playerMovement.rb.AddForce(playerMovement.Movement * dashForce, ForceMode.VelocityChange);
        
        //if(playerMovement.MovementX == 0 && playerMovement.MovementY == 0 )
        //playerMovement.Movement = playerMovement.CamF;
        //playerMovement.rb.AddForce(playerMovement.Movement * dashForce, ForceMode.VelocityChange);

        //friction stop
        GetComponent<Collider>().material.dynamicFriction = 0;
        GetComponent<Collider>().material.staticFriction = 0;
        playerMovement.MaxSpeed = 30;
        
        //on collision enter stop
        //no charge when cooldown + cant charge if already holding shift = fix this!
        //create circle radius to show AoE around player
        //enemies = hit, effect on them show they are hit + they bounce
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
