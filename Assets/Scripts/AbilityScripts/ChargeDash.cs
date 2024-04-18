using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeDash : MonoBehaviour
{

    public KeyCode crouchKey;

    public float startTime = 0f;
    public float holdTime = 3.0f;

    //charge stats
    public float timeBetweenCharge, chargeActivationTime;

    //bools
    bool chargeAcquired, chargeAble, charging, chargeGo, chargeCooldown;

    void bruh()
    {
        //find the position using raycast
        //Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //just a ray through the middle of your screen
        //RaycastHit hit;

        //check if ray hits something
        //Vector3 targetPoint;
        //if (Physics.Raycast(ray, out hit))
        //    targetPoint = hit.point;
        //else
        //    targetPoint = ray.GetPoint(75); //just a point far away from the player

        //calculate direction from attackPoint to targetPoint
        //Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //caculate spread
        //float x = Random.Range(-spread, spread);
        //float y = Random.Range(-spread, spread);

        //calculate new direction with spread
        //Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //just add spread to last direction
        
        //Instantiate bullet/projectile
        //GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet/projectile

        //rotate bullet to shoot direction
        //currentBullet.transform.forward = directionWithSpread.normalized;

        //add forces to bullet
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //instantiate muzzle flash, if you have one
        //if (muzzleFlash != null)
        //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        //bulletsLeft--;
        //bulletsShot++;

        //invoke resetShot function (if not already invoked), with your timeBetweenShooting
        //if (allowInvoke)
        //{
            //Invoke("Resetshot", timeBetweenShooting);
            //allowInvoke = false;

            //add recoil to the player
        //playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        //}

        //if more than one bulletsPerTap make sure to repeat shoot function
        //if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            //Invoke("Shoot", timeBetweenShots);
    }

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

        //change charge cooldown to after you collide
        chargeCooldown = true;

        //move forwards
        //raycast?

        //run chargecooldown
        StartCoroutine (ChargeCooldown());

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