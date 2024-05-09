using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveNuke : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePosition;
    public float nukeForce;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //BulletPrefab.AddForce(nukeForce, ForceMode.VelocityChange);
            GameObject bullet = Instantiate(BulletPrefab, FirePosition.position , Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.right * 1000);
        }
    }
}
