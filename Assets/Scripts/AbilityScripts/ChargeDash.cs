using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeDash : MonoBehaviour
{

    [Header("References")]
    //public Transform orientation;
    //public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("CameraEffects")]
    //public playercam cam;
    public float dashFov;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashkey = KeyCode.E;

    public KeyCode crouchKey;

    public float startTime = 0f;
    public float holdTime = 3.0f;

    //charge stats
    public float timeBetweenCharge, chargeActivationTime;

    //bools
    bool chargeAcquired, chargeAble, charging, chargeGo, chargeCooldown;

    void Awake()
    {
        chargeAble = true;
        charging = false;
        chargeGo = false;
        chargeCooldown = false;
    }

    void Update()
    {
        ChargeInput();
    }

    void ChargeInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && chargeAble && !charging && !chargeCooldown)
        {
            ChargingUp();
        }
    }

    void ChargingUp()
    {
        Debug.Log("Charging");
        charging = true;

        //crouch
        //slow down movements

        //if(chargeActivationTime <= 0)
        //{
            ChargeGo();
        //}
    }

    void ChargeGo()
    {
        Debug.Log("Going");
        chargeAble = false;
        charging = false;
        chargeGo = true;

        //Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        //delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);


        Invoke(nameof(ResetDash), dashDuration);

        //change charge cooldown to after you collide
        chargeCooldown = true;

        //move forwards
        //raycast?

        //run chargecooldown
        StartCoroutine (ChargeCooldown());

    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
    
    }

    private void ResetDash()
    {
        //PlayMode.dashing = false;

        //Camera.DoFov(85f);
    }

    public IEnumerator ChargeCooldown()
    {
        chargeGo = false;
        Debug.Log("waiting");
        yield return new WaitForSeconds(3);
        chargeAble = true;
        chargeCooldown = false;
        Debug.Log("able");
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Crouched");
            transform.localScale = new Vector3(1, 0.5f, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }

        if (context.canceled)
        {
            Debug.Log("Uncrouched");
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }
}

//public KeyCode crouchKey;

    //public float startTime = 0f;
    //public float holdTime = 3.0f;

    //charge stats
    //public float timeBetweenCharge, chargeActivationTime;

    //bools
    //bool chargeAcquired, chargeAble, charging, chargeGo, chargeCooldown;
