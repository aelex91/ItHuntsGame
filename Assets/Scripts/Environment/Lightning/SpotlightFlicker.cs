using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class SpotlightFlicker : MonoBehaviour
{
    private Light spotlight;

    [Header("Cycle Settings")]
    public float stableDuration = 5f; // How long the light stays fully on
    public float flickerDuration = 0.5f; // How long the light flickers
    public float blackoutDuration = 1f; // How long the light is completely off

    [Header("Flicker Settings")]
    public float flickerSpeed = 0.05f;
    public float flickerMin = 1f;
    public float flickerMax = 1.2f;

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
