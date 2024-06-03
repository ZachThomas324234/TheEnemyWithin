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
    private PostProcessingManager ppm;
    public GameObject groundPoundText;

    [Header("Properties")]
    [Range(0, 1.5f)]public float groundPoundCharge;
    [Range(0, 1.5f)]public float groundPoundCooldown;
    [Range(0, 0.2f)]public float height;
    public float jumpForce;

    void Awake()
    {
      groundPoundText.SetActive(false);
      td = FindAnyObjectByType<TestDash>();
      playerMovement = GetComponent<PlayerMovement>();
      rb = GetComponent<Rigidbody>();
      ppm = FindAnyObjectByType<PostProcessingManager>();
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

        ppm.targetVignette = 0.2f;
        ppm.targetLensDistortion = -0.7f;
      }
      else if(context.canceled && playerMovement.hasGroundPound)
      {
        holdingCtrl = false;

        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

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
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;
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
        
        Invoke("groundFloor", 0.2f);
    }

    private void groundFloor()
    {
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
