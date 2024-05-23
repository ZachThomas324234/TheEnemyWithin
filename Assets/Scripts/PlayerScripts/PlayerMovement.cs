using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Properties")]
    public float Speed, normalJumpForce;
    public float MaxSpeed = 12;
    [Range(0, 2f)]public float staminaAmount = 0;
    [Range(0, 2f)]public float staminaCooldown = 0;
    [HideInInspector]public Vector3 CamF;
    [HideInInspector]public Vector3 CamR;
    [HideInInspector]public Vector3 Movement;
    [HideInInspector]public float MovementX;
    [HideInInspector]public float MovementY;

    [Header("References")]
    public Rigidbody rb;
    public Transform Camera;

    //public GameObject dashText;
    //public GameObject nukeText;
    //public GameObject lightingText;
    //public GameObject teleText;
    //public GameObject groundPoundText;

    public TestDash td;
    public gunScript gs;
    public bullet bullet;

    [Header("States")]
    public bool hasDash = false;
    public bool hasRadioactiveNuke = false;
    public bool hasLightningStrike = false;
    public bool hasTeleport = false;
    public bool hasGroundPound = false;
    public bool jumping = false;
    public bool grounded;
    public bool crouching;
    public bool isRunning;
    public bool cantRun;


    void Awake()
    {
        Camera = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        td = GetComponent<TestDash>();
        gs = GetComponent<gunScript>();
        bullet = GetComponent<bullet>();
        staminaAmount = 2;

        //dashText.SetActive(false);
        //nukeText.SetActive(false);
        //lightingText.SetActive(false);
        //teleText.SetActive(false);
        //groundPoundText.SetActive(false);
    }

    void FixedUpdate()
    {
        CamF = Camera.forward;
        CamR = Camera.right;
        CamF.y = 0;
        CamR.y = 0;
        CamF = CamF.normalized;
        CamR = CamR.normalized;

        if (td.Dashing) return;

        Movement = (CamF * MovementY + CamR * MovementX).normalized;
        rb.AddForce(Movement * Speed);
        rb.AddForce(rb.velocity * -6f);

        LockToMaxSpeed();
    }

    public void onMove(InputAction.CallbackContext MovementValue)
    {
        Vector2 inputVector = MovementValue.ReadValue<Vector2>();
        MovementX = inputVector.x;
        MovementY = inputVector.y;
    }

    public void Run(InputAction.CallbackContext run)
    {
        if(run.started && !td.Dashing && !crouching && !cantRun)
        {
            isRunning = true;
            Speed = 300;
        }
        if(run.canceled && !td.Dashing && !crouching)
        {
            isRunning = false;
            Speed = 70;
        }
    }

    public void LockToMaxSpeed()
    {
        Vector3 newVelocity = rb.velocity;
        newVelocity.y = 0f;
        newVelocity = Vector3.ClampMagnitude(newVelocity, MaxSpeed);
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }

    public void Jump(InputAction.CallbackContext jump)
    {
        if (jump.started && !jumping && !td.Dashing && !crouching && grounded)
        {
            jumping = true;
            rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * normalJumpForce, ForceMode.VelocityChange);
        }

        if (jump.canceled && !td.Dashing && !crouching)
        {
            jumping = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gs.ableToShoot = false;
            gs.gunPutAway.Play("gunPutAway");
            crouching = true;
            transform.localScale = new Vector3(1, 0.5f, 1);
            Speed = 20;
        }

        if (context.canceled)
        {
            gs.ableToShoot = true;
            gs.gunBringBack.Play("gunBringBack");
            crouching = false;
            transform.localScale = new Vector3(1, 1, 1);
            Speed = 70;
        }
    }
}
