using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundPound : MonoBehaviour
{
    public bool holdingCtrl, jumping;
    public bool groundPoundDone = false;

    [Header("References")]
    private PlayerMovement playerMovement;
    private Rigidbody rb;
    private TestDash td;

    [Header("Properties")]
    [Range(0, 1.5f)]public float groundPoundCharge;
    [Range(0, 1.5f)]public float groundPoundCooldown;
    [Range(0, 0.2f)]public float height;
    public float jumpForce;

    void Awake()
    {
        td = FindAnyObjectByType<TestDash>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(holdingCtrl && groundPoundCooldown == 0)groundPoundCharge += Time.deltaTime;
        else groundPoundCharge -= Time.deltaTime;
        
        groundPoundCharge = Math.Clamp (groundPoundCharge, 0, 1.5f);

        if (groundPoundCharge == 1.5f && groundPoundCooldown == 0 && !groundPoundDone)
        {
          groundPoundDone = true;
        }

        if(groundPoundCooldown > 0) groundPoundCooldown -= Time.deltaTime;
        if (groundPoundCooldown < 0) groundPoundCooldown = 0;

        if (jumping)
        {
            height += Time.deltaTime;
        }

        if (height >= 0.2f)
        {
            ResetGroundPound();
        }
    }

    public void chargingGroundPound(InputAction.CallbackContext context)
    {
      if(context.started && !jumping && groundPoundCooldown <= 0 && playerMovement.hasGroundPound)
      {
        Debug.Log("g");
        holdingCtrl = true;
      }
      else if(context.canceled && playerMovement.hasGroundPound)
      {
        holdingCtrl = false;

        //if(!jumping)
        //global volume
        //targetVignette = 0;
        //targetLensDistortion = 0;

        if (groundPoundCharge >= 1.5f)
        {
          JumpingHigh();
          groundPoundCooldown = 1.5f;
        }
        else groundPoundCharge = 0;
      }
    }

    public void JumpingHigh()
    {
        Debug.Log("Jump");
        jumping = true;

        playerMovement.Movement = playerMovement.CamF;
        playerMovement.rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void ResetGroundPound()
    {
        Debug.Log("Doing your mom");
        rb.AddForce(Vector3.down * jumpForce, ForceMode.VelocityChange);
        jumping = false;
        groundPoundDone = false;
        height = 0;
    }

    //public virtual void OnCollisionEnter (Collision collision)
    //{
    //  
    //  if (jumping)
    //  {
    //    if(collision.contacts[0].normal.y > 0.85f)
    //    {
    //        ResetGroundPound();
    //        Debug.Log("haloo");
    //    }
    //  }
    //}

}
