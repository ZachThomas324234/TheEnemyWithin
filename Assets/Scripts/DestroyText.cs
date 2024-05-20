using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class DestroyText : MonoBehaviour
{
    public bool destroy;
    public float destroyAfter = 1;
    public float Age;

    public PlayerMovement pm;

    void Awake()
    {
        pm = FindAnyObjectByType<PlayerMovement>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (destroy)
        {
            Age += Time.deltaTime;
            if (Age >= destroyAfter)
            {
                Debug.Log("hello");
                gameObject.SetActive(false);
                Age = 0;
            }
        }
    }
}
