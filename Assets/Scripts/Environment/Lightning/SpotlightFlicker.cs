using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class SpotlightFlicker : MonoBehaviour
{
    private Light spotlight;

    [Header("Cycle Settings")]
    public float stableDuration = 5f; // How long the light stays fully on
    public float flickerDuration = 1.2f; // How long the light flickers
    public float blackoutDuration = 3f; // How long the light is completely off

    [Header("Flicker Settings")]
    public float flickerSpeed = 0.02f;
    public float flickerMin = 0.4f;
    public float flickerMax = 1.4f;

    void Start()
    {
        spotlight = GetComponent<Light>();
        StartCoroutine(LightLoop());
    }

    IEnumerator LightLoop()
    {
        while (true)
        {
            // 1. Light stays on
            spotlight.intensity = flickerMax;
            yield return new WaitForSeconds(stableDuration);

            // 2. Flicker
            float flickerEndTime = Time.time + flickerDuration;
            while (Time.time < flickerEndTime)
            {
                spotlight.intensity = Random.Range(flickerMin, flickerMax);
                yield return new WaitForSeconds(flickerSpeed);
            }

            // 3. Blackout
            spotlight.intensity = 0f;
            yield return new WaitForSeconds(blackoutDuration);

            // 4. Back to full light
            spotlight.intensity = flickerMax;
            // Loop will restart at stableDuration
        }
    }
}
