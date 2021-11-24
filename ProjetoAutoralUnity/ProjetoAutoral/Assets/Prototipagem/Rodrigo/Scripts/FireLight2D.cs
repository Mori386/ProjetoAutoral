using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FireLight2D : MonoBehaviour
{
    Light2D light2D;
    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }
    public IEnumerator LightIntensityWave()
    {
        float intensityStart = light2D.intensity;
        while (true)
        {
            light2D.intensity = Random.Range(intensityStart - 0.2f, intensityStart - 0.4f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
