using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioActiveNuke : MonoBehaviour
{

    private PostProcessingManager ppm;
    // Start is called before the first frame update
    void Awake()
    {
        ppm = FindAnyObjectByType<PostProcessingManager>();
        huh();
    }

    // Update is called once per frame
    void Update()
    {
        ppm.targetVignette = 0.2f;
        ppm.targetLensDistortion = -0.7f;
    }

    public void huh()
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
