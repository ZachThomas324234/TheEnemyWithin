using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public Volume volume;
    public Vignette vignette;
    public LensDistortion lensDistortion;

    public float targetLensDistortion;
    private float blendLensDistortion;
    public float targetVignette;
    private float blendVignette;

    void Awake()
    {
      volume = FindAnyObjectByType<Volume>();
      volume.profile.TryGet (out Vignette v);
      vignette = v;
      volume.profile.TryGet (out LensDistortion lD);
      lensDistortion = lD;
    }

    public void Update()
    {
        vignette.intensity.value = Mathf.SmoothDamp(vignette.intensity.value, targetVignette, ref blendVignette, 0.5f);
        lensDistortion.intensity.value = Mathf.SmoothDamp(lensDistortion.intensity.value, targetLensDistortion, ref blendLensDistortion, 0.5f);
    }
}
