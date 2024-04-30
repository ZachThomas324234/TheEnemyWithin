using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TestDash : MonoBehaviour
{

    public bool holdingShift;
    public bool Dashing;
    [Range(0, 1.5f)]public float DashCharge;
    [Range(0, 1.5f)]public float DashCooldown;
    [Range(0, 0.5f)]public float DashDuration;
    public Transform orientation;
    public float dashForce;
    public float dashUpwardForce;
    public PlayerMovement playerMovement;
    public Rigidbody rb;

    public AudioSource chargeDashFire, startCharge, chargePowerUp, chargeExplosion, powerDown, chargeDone;

    //Global Volume
    public Volume volume;
    public Vignette vignette;
    public LensDistortion lensDistortion;

    public float targetLensDistortion;
    public float blendLensDistortion;

    public float targetVignette;
    public float blendVignette;

    void Update()
    {
        if(holdingShift && DashCooldown == 0)DashCharge += Time.deltaTime;
        else DashCharge -= Time.deltaTime;

        DashCharge = Math.Clamp (DashCharge, 0, 1.5f);

        if(DashCooldown > 0) DashCooldown -= Time.deltaTime;
        if (DashCooldown < 0) DashCooldown = 0;

        vignette.intensity.value = Mathf.SmoothDamp(vignette.intensity.value, targetVignette, ref blendVignette, 0.5f);
        lensDistortion.intensity.value = Mathf.SmoothDamp(lensDistortion.intensity.value, targetLensDistortion, ref blendLensDistortion, 0.5f);
    }

    void Awake()
    {
      volume.profile.TryGet (out Vignette v);
      vignette = v;
      volume.profile.TryGet (out LensDistortion lD);
      lensDistortion = lD;
    }

    public void OnShift(InputAction.CallbackContext context)
    {
      if(context.started)
      {
        holdingShift = true;
        chargeDashFire.Play();
        chargePowerUp.Play();
        powerDown.Stop();
        targetVignette = 0.2f;
        targetLensDistortion = -0.7f;
      }
      else if(context.canceled)
      {
        chargeDashFire.Stop();
        chargePowerUp.Stop();
        powerDown.Play();
        holdingShift = false;
        targetVignette = 0;
        targetLensDistortion = 0;
        if (DashCharge >= 1.5f)
        {
            chargeDone.Play();
            Dash();
            DashCooldown = 1.5f;
        }
        else DashCharge = 0;
      }
    }

    private void Dash()
    {
        Debug.Log("Dashing");
        startCharge.Play();
        Dashing = true;
        targetVignette = 0;
        targetLensDistortion = 0;
        if(playerMovement.MovementX == 0 && playerMovement.MovementY == 0 )
          playerMovement.Movement = playerMovement.CamF;
        playerMovement.rb.AddForce(playerMovement.Movement * dashForce, ForceMode.VelocityChange);

        //friction stop
        GetComponent<Collider>().material.dynamicFriction = 0;
        GetComponent<Collider>().material.staticFriction = 0;
        playerMovement.MaxSpeed = 30;
        //disable player movement
        
        //on collision enter stop
    }

    public virtual void OnCollisionEnter (Collision collision)
    {
      if (Dashing)
      {
      if (collision.collider.CompareTag("Enemy"))
      {
        Debug.Log("Collision on Enemy");
        ResetDash();
        chargeExplosion.Play();
        chargeDashFire.Stop();
        //collision.contacts[0].normal
        //is it at an angle. then wall.
      }
      }
    }

    private void ResetDash()
    {
      Dashing = false;
      Debug.Log("Resetting dash");
      GetComponent<Collider>().material.dynamicFriction = 0.75f;
      GetComponent<Collider>().material.staticFriction = 0.75f;
      playerMovement.MaxSpeed = 10;
    }
}
