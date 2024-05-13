using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System;

public class RadioactiveNuke : MonoBehaviour
{

    //time inbetween shots
    //holding shoot
    //press left click with no bullets reloads
    #region variables
    public float bulletSpeed;
    public float nukeForce;
    
    public float magazineSize, bulletsPerTap, bulletsLeft;

    public bool reloadActive, gunReloading, isShooting;

    public float timeBetweenShots;

    public Transform FirePosition;
    public GameObject BulletPrefab;
    private PlayerMovement player;
    public TextMeshProUGUI ammunitionDisplay;
    public AudioSource reloading;
    public Animator gunReload;
    #endregion

    void Awake()
    {
        timeBetweenShots = 1;
        reloadActive = false;
        gunReloading = false;
        magazineSize = 10;
        bulletsLeft = magazineSize;
        player = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        //timeBetweenShots = Math.Clamp(timeBetweenShots ,0 , 1);
        //timeBetweenShots += Time.deltaTime;

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(magazineSize / bulletsPerTap + " / " + bulletsLeft / bulletsPerTap);

        //if (!isShooting)
        //{
        //    timeBetweenShots += Time.deltaTime;
        //}
        //else
        //{
        //    timeBetweenShots -= Time.deltaTime;
        //}

        if (Input.GetMouseButtonDown(0) && magazineSize >= 1 && !gunReloading)
        {
            var bullet = Instantiate(BulletPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = player.Camera.forward * bulletSpeed;
            Shoot();
            //BulletPrefab.AddForce(nukeForce, ForceMode.VelocityChange);
            //GameObject bullet = Instantiate(BulletPrefab, FirePosition.position , Quaternion.identity);
            //bullet.GetComponent<Rigidbody>().AddForce(transform.right * 1000);
        }

        if (Input.GetKeyDown(KeyCode.R) && magazineSize <= 9 && !reloadActive && !gunReloading && !isShooting)
        {
            gunReload.Play("gunReload");
            gunReloading = true;
            Invoke("Reload", 1);
            reloading.Play();
        }

        if (Input.GetMouseButtonDown(0) && magazineSize <= 0 && !reloadActive && !gunReloading && !isShooting)
        {
            gunReload.Play("gunReload");
            reloadActive = true;
            reloading.Play();
            Invoke("Reload", 1);
        }
    }

    public void Shoot()
    {
        isShooting = true;
        magazineSize -= 1;
        ResetShoot();
    }

    public void ResetShoot()
    {
        isShooting = false;
    }

    public void Reload()
    {
        magazineSize = 10;
        reloadActive = false;
        gunReloading = false;
    }
}
