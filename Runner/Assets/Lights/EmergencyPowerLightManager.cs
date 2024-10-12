using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyPowerLightManager : MonoBehaviour
{
    public List<Light> spotLights;
    public List<Light> pointLights;

    public float maxIntensity = 15f;
    public float fadeDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpotLightLoopIntensity());
        StartCoroutine(PointLightLoopIntensity());
    }

    private IEnumerator SpotLightLoopIntensity() 
    {
        while (true) 
        {
            yield return StartCoroutine(SpotLightFadeIntensity(13f, maxIntensity));

            yield return StartCoroutine(SpotLightFadeIntensity(maxIntensity, 13f));
        }
    }

    private IEnumerator PointLightLoopIntensity()
    {
        while (true)
        {
            yield return StartCoroutine(PointLightFadeIntensity(.2f, .5f));

            yield return StartCoroutine(PointLightFadeIntensity(.5f, .2f));
        }
    }

    private IEnumerator SpotLightFadeIntensity(float startIntensity, float endIntensity) 
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) 
        {
            float intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / fadeDuration);

            foreach (Light light in spotLights) 
            {
                light.intensity = intensity;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Light light in spotLights) 
        {
            light.intensity = endIntensity;
        }
    }

    private IEnumerator PointLightFadeIntensity(float startIntensity, float endIntensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / fadeDuration);

            foreach (Light light in pointLights)
            {
                light.intensity = intensity;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Light light in pointLights)
        {
            light.intensity = endIntensity;
        }
    }
}
