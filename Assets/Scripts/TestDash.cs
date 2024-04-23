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
        playerMovement.rb.AddForce(playerMovement.Movement * dashForce, ForceMode.VelocityChange);
        //friction stop
        //on collision enter stop
        //disable player movement
    }
}
