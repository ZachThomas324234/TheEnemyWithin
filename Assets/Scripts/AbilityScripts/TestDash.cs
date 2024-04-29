using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public AudioSource chargeDashFire;
    public AudioSource startCharge;
    public AudioSource chargePowerUp;
    public AudioSource chargeExplosion;

    void Update()
    {
        if(holdingShift && DashCooldown == 0)DashCharge += Time.deltaTime;
        else DashCharge -= Time.deltaTime;

        DashCharge = Math.Clamp (DashCharge, 0, 1.5f);

        if(DashCooldown > 0) DashCooldown -= Time.deltaTime;
        if (DashCooldown < 0) DashCooldown = 0;
    }

    public void OnShift(InputAction.CallbackContext context)
    {
      if(context.started)
      {
        holdingShift = true;
        chargeDashFire.Play();
        chargePowerUp.Play();
        //while charging = true, chargepowerup.play();
      }
      else if(context.canceled)
      {
        holdingShift = false;
        if (DashCharge >= 1.5f)
        {
            Dash();
            DashCooldown = 1.5f;
        }
      }
    }

    private void Dash()
    {
        Debug.Log("Dashing");
        startCharge.Play();
        Dashing = true;
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
