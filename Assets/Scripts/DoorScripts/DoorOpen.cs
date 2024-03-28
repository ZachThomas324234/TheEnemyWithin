using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    bool doorIsOpen;

    private void Start()
    {
        Debug.Log("door is closed");
        doorIsOpen = false;
    }

    private void Update()
    {

    }



    private void TheDoorOpens()
    {
        Debug.Log("door is open");
        doorIsOpen = true;
    }

}
