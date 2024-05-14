using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System;

public class gunScript : MonoBehaviour
{

    //time inbetween shots
    //holding shoot
    //press left click with no bullets reloads
    #region variables
    public float bulletSpeed;
    
    public float magazineSize, bulletsPerTap, bulletsLeft;

    public bool reloadActive, gunReloading, isShooting, ableToShoot;

    public float timeBetweenShots;

    public Transform FirePosition;
    public GameObject BulletPrefab;
    private PlayerMovement player;
    private TestDash td;
    public TextMeshProUGUI ammunitionDisplay;
    public AudioSource reloading;
    public AudioSource gunShot;
    public Animator gunReload, gunPutAway, gunBringBack;
    #endregion

    void Awake()
    {
        timeBetweenShots = 1;
        reloadActive = false;
        gunReloading = false;
        ableToShoot = true;
        magazineSize = 10;
        bulletsLeft = magazineSize;
        player = FindAnyObjectByType<PlayerMovement>();
        td = GetComponent<TestDash>();
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

        if (Input.GetMouseButtonDown(0) && magazineSize >= 1 && !gunReloading && !isShooting && ableToShoot && !td.Dashing)
        {
            var bullet = Instantiate(BulletPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = player.Camera.forward * bulletSpeed;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && magazineSize <= 9 && !reloadActive && !gunReloading && !isShooting && !td.holdingShift && !td.Dashing)
        {
            ReloadAnimation();
        }

        if (Input.GetMouseButtonDown(0) && magazineSize <= 0 && !reloadActive && !gunReloading && !isShooting && !td.holdingShift && !td.Dashing)
        {
            ReloadAnimation();
        }
    }

    public void ReloadAnimation()
    {
        gunReload.Play("gunReload");
        gunReloading = true;
        reloadActive = true;
        reloading.Play();
        Invoke("Reload", 1);
    }

    public void Shoot()
    {
        gunShot.Play();
        isShooting = true;
        magazineSize -= 1;
        Invoke("ResetShoot", 0.5f);
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
