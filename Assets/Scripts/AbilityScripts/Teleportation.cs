using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleportation : MonoBehaviour
{
    private PostProcessingManager ppm;
    private PlayerMovement playerMovement;

    public bool holdingCtrl, teleDone = false;

    [Header("Properties")]
    [Range(0, 1.5f)]public float teleCharge;
    [Range(0, 1.5f)]public float teleCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        ppm = FindAnyObjectByType<PostProcessingManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingCtrl && teleCooldown == 0)teleCharge += Time.deltaTime;
        else teleCharge -= Time.deltaTime;
        
        teleCharge = Math.Clamp (teleCharge, 0, 1.5f);

        if (teleCharge == 1.5f && teleCooldown == 0 && !teleDone)
        {
          teleDone = true;
          //chargeDoneSFX.Play();
        }

        if(teleCooldown > 0) teleCooldown -= Time.deltaTime;
        if (teleCooldown < 0) teleCooldown = 0;
    }

    public void OnCtrl(InputAction.CallbackContext context)
    {
      if(context.started && teleCooldown <= 0 && playerMovement.hasTeleport)
      {
        holdingCtrl = true;
        //audio
        //chargeDashFire.Play();
        //chargePowerUp.Play();
        //powerDown.Stop();
        
        //global volume
        ppm.targetVignette = 0.2f;
        ppm.targetLensDistortion = -0.7f;
      }
      else if(context.canceled && playerMovement.hasTeleport)
      {
        holdingCtrl = false;
        //audio
        //chargeDashFire.Stop();
        //chargePowerUp.Stop();
        //if(!Dashing)powerDown.Play();
        //global volume
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

        if (teleCharge >= 1.5f)
        {
          teleport();
          teleCooldown = 1.5f;
        }
        else teleCharge = 0;
      }
    }

    public void teleport()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ppm.targetVignette = 0.2f;
            ppm.targetLensDistortion = -0.7f;
        }
        else
        {
            ppm.targetVignette = 0;
            ppm.targetLensDistortion = 0;
        }
    }
}
