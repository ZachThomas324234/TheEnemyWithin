using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDash : MonoBehaviour
{

    bool holdingShift;
    float DashCharge;
    float DashCooldown;

    void Update()
    {
        if(holdingShift)DashCharge += Time.deltaTime;
        else DashCharge -= Time.deltaTime;

    DashCharge = Math.Clamp (DashCharge, 0, 1);

        if(DashCooldown > 0 && DashCharge > 0) DashCharge -= Time.deltaTime;
        if (DashCooldown < 0) DashCharge = 0;
    }

    //void OnShift()
    //{
        //if(context.started)
        //{
            //holdingShift = true;
        //}
        //else
        //{
            //if (DashCharge> 1)
            //Dash();
        //}
    //}

    //private void Dash()
    //{
        
    //}
}
