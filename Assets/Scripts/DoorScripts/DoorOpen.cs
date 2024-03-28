using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    public bool doorIsOpen;

    public float Speed;

    private Vector3 OriginPosition;
    public Vector3 OpenPosition;
    private Vector3 TargetPosition;

    private PlayerMovement playerMovement;


    private void Start()
    {
        OriginPosition = transform.position;
        TargetPosition = OriginPosition;
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        Debug.Log("door is closed");
        doorIsOpen = false;
    }

    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, TargetPosition, Speed);
        if(Vector3.Distance(OriginPosition, playerMovement.transform.position) < 5)
        {
            if(!doorIsOpen)TheDoorOpens();
        }
        else
        {
            if(doorIsOpen)TheDoorCloses();
        }
    }

    private void TheDoorCloses()
    {
        Debug.Log("door is closed");
        doorIsOpen = false;
        TargetPosition = OriginPosition;
    }

    private void TheDoorOpens()
    {
        Debug.Log("door is open");
        doorIsOpen = true;
        TargetPosition += OpenPosition;
    }

}
