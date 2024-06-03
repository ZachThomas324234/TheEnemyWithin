using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioActiveNuke : MonoBehaviour
{
    private PostProcessingManager ppm;
    private PlayerMovement playerMovement;

    public bool holdingCtrl, nukeDone = false;

    [Header("Properties")]
    [Range(0, 1.5f)]public float nukeCharge;
    [Range(0, 1.5f)]public float nukeCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        ppm = FindAnyObjectByType<PostProcessingManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingCtrl && nukeCooldown == 0)nukeCharge += Time.deltaTime;
        else nukeCharge -= Time.deltaTime;
        
        nukeCharge = Math.Clamp (nukeCharge, 0, 1.5f);

        if (nukeCharge == 1.5f && nukeCooldown == 0 && !nukeDone)
        {
          nukeDone = true;
          //chargeDoneSFX.Play();
        }

        if(nukeCooldown > 0) nukeCooldown -= Time.deltaTime;
        if (nukeCooldown < 0) nukeCooldown = 0;
    }

    public void OnCtrl(InputAction.CallbackContext context)
    {
      if(context.started && nukeCooldown <= 0 && playerMovement.hasRadioactiveNuke)
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
      else if(context.canceled && playerMovement.hasRadioactiveNuke)
      {
        holdingCtrl = false;
        //audio
        //chargeDashFire.Stop();
        //chargePowerUp.Stop();
        //if(!Dashing)powerDown.Play();
        //global volume
        ppm.targetVignette = 0;
        ppm.targetLensDistortion = 0;

        if (nukeCharge >= 1.5f)
        {
          nuke();
          nukeCooldown = 1.5f;
        }
        else nukeCharge = 0;
      }
    }

    public void nuke()
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
