using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    //private PlayerMovementDashing pm;


    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;


    [Header("CameraEffects")]
    //public PlayerCam cam;
    public float dashFov;


    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;


    [Header("Input")]
    public KeyCode dashkey = KeyCode.E;


    //Movement Script
    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public bool dashing;

    private float desiredMoveSpeed;
    private float lastdesiredMoveSpeed;
    private bool keepMomentum;


    private void StateHandler()
        {
            //Mode - Dashing
            if (dashing)
            {

                desiredMoveSpeed = dashSpeed;
                speedChangefactor = dashSpeedChangeFactor;
            }


            bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastdesiredMoveSpeed;
            //if (lastState == MovementState.dashing) keepMomentum = true;


            if(desiredMoveSpeedHasChanged)
            {
                if (keepMomentum)
                {
                    StopAllCoroutines();
                    //StartCoroutine(SmoothlyLerpMoveSpeed());
                }
                else
                {
                    StopAllCoroutines();
                    //moveSpeed = desiredMoveSpeed;
                }
            }


            lastdesiredMoveSpeed = desiredMoveSpeed;
            //lastState = state;
        }


    private float speedChangefactor;


    //private IEnumerator SmoothlyLerpMoveSpeed()
    //{
        //smoothly lerp movementSpeed to desired value
        //float time = 0;
        //float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        //float startValue = moveSpeed;


        //float boostFactor = speedChangefactor;


        //while (time < difference)
        //{
            //moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);


            //time += Time.deltaTime * boostFactor;


            //yield return null;
        //}


        //moveSpeed = desiredMoveSpeed;
        //speedChangefactor = 1f;
        ///keepMomentum = false;
    //}


    //in update
    //handle drag
    //if(state == MovementState.walking || state == MovementState.sprinting. || state == MovementState.crouching)
        //rb.drag = groundDrag;
    //else
        //rb.drag = 0;
    //Movement Script


    //Camera Script
    //Tweening
    public void DoFov(float endValue)
    {
        //GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }


    //Camera Script


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //pm = GetComponent<PlayerMovementDashing>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(dashkey))
            Dash();


        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }


    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;


        ///pm.dashing = true;


        //cam.DoFov(dashFov);


        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;


        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);


        Invoke(nameof(ResetDash), dashDuration);
    }


    private Vector3 delayedForceToApply;


    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }


    private void ResetDash()
    {
        //pm.dashing = false;


        //cam.DoFov(85f);
    }
}