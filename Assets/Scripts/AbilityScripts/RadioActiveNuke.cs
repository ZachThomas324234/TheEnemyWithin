using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioActiveNuke : MonoBehaviour
{

    public bool holdingCtrl, pressRClick = false, nukeDone = false;

    [Header("References")]
    public GameObject NukePrefab;
    public Transform FirePosition;
    private PostProcessingManager ppm;
    private PlayerMovement playerMovement;
    public GameObject nukeGrow, dashFX;
    public Camera camera;

    [Header("Properties")]
    [Range(0, 1.5f)]public float nukeCharge;
    [Range(0, 1.5f)]public float nukeCooldown;
    public float nukeSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        ppm = FindAnyObjectByType<PostProcessingManager>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
      if(pressRClick && nukeCooldown == 0)nukeCharge += Time.deltaTime;
      else nukeCharge -= Time.deltaTime;
        //if(holdingCtrl && nukeCooldown == 0)nukeCharge += Time.deltaTime;
        
        nukeCharge = Math.Clamp (nukeCharge, 0, 1.5f);

        if (nukeCharge == 1.5f && nukeCooldown == 0 && !nukeDone)
        {
          nukeDone = true;
          //chargeDoneSFX.Play();
        }

        if(nukeCooldown > 0) nukeCooldown -= Time.deltaTime;
        if (nukeCooldown < 0) nukeCooldown = 0;
    }

    public void OnRClick(InputAction.CallbackContext context)
    {
      if(context.started && nukeCooldown <= 0 && playerMovement.hasRadioactiveNuke)
      {
        pressRClick = true;
        //holdingCtrl = true;
        //audio
        //chargeDashFire.Play();
        //chargePowerUp.Play();
        //powerDown.Stop();
        nukeGrow = Instantiate(NukePrefab, FirePosition.position, FirePosition.rotation, playerMovement.Camera);
        Animator nukeAnim = nukeGrow.GetComponent<Animator>();
        nukeAnim.Play("nukeGrow");
        GameObject particle = Instantiate(dashFX, transform.position, Quaternion.identity, playerMovement.Camera);
        
        //global volume
        ppm.targetVignette = 0.2f;
        ppm.targetLensDistortion = -0.7f;
      }
      else if(context.canceled && playerMovement.hasRadioactiveNuke)
      {
        pressRClick = false;

        Destroy(nukeGrow);
        //Destroy(particle);
        //holdingCtrl = false;
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
    }
}
