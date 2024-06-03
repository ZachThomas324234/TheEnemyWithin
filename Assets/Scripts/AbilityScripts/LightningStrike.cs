using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightningStrike : MonoBehaviour
{
    private PostProcessingManager ppm;
    private PlayerMovement playerMovement;

    public bool holdingCtrl, lightningDone = false;

    [Header("Properties")]
    [Range(0, 1.5f)]public float lightningCharge;
    [Range(0, 1.5f)]public float lightningCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        ppm = FindAnyObjectByType<PostProcessingManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingCtrl && lightningCooldown == 0)lightningCharge += Time.deltaTime;
        else lightningCharge -= Time.deltaTime;
        
        lightningCharge = Math.Clamp (lightningCharge, 0, 1.5f);

        if (lightningCharge == 1.5f && lightningCooldown == 0 && !lightningDone)
        {
          lightningDone = true;
          //chargeDoneSFX.Play();
        }

        if(lightningCooldown > 0) lightningCooldown -= Time.deltaTime;
        if (lightningCooldown < 0) lightningCooldown = 0;
    }

    public void OnCtrl(InputAction.CallbackContext context)
    {
      if(context.started && lightningCooldown <= 0 && playerMovement.hasLightningStrike)
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
      else if(context.canceled && playerMovement.hasLightningStrike)
      {
        holdingCtrl = false;
        //audio
        //chargeDashFire.Stop();
        //chargePowerUp.Stop();
        //if(!Dashing)powerDown.Play();
        //global volume
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

        if (lightningCharge >= 1.5f)
        {
          lightning();
          lightningCooldown = 1.5f;
        }
        else lightningCharge = 0;
      }
    }

    public void lightning()
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
