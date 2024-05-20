using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public PlayerMovement playerMovement;

    public void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == playerMovement.gameObject) return;
        playerMovement.grounded = true;
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == playerMovement.gameObject) return;
        playerMovement.grounded = false;
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == playerMovement.gameObject) return;
        playerMovement.grounded = true;
    }
}
