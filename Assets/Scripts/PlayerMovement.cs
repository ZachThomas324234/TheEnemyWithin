using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float Speed;
    public float MaxSpeed = 12;
    [HideInInspector]public Vector3 CamF;
    [HideInInspector]public Vector3 CamR;
    [HideInInspector]public Vector3 Movement;
    [HideInInspector]public float MovementX;
    [HideInInspector]public float MovementY;

    public Rigidbody rb;
    public Transform Camera;

    public TestDash td;


    void Awake()
    {
        Camera = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        td = GetComponent<TestDash>();
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

    public void LockToMaxSpeed()
    {
        // Get the velocity direction
        Vector3 newVelocity = rb.velocity;
        newVelocity.y = 0f;
        newVelocity = Vector3.ClampMagnitude(newVelocity, MaxSpeed);
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started && !td.Dashing)
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
