using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class SpeedEffectsController : MonoBehaviour
{
    public float minSpeedCaptured = 15f;
    public float maxSpeedCaptured = 25f;
    public float maxDecayPercentage = 0.5f;
    public float maxIncreasePercentage = 2f;

    public float lastRatio = 0f;

    public Volume volume;
    public Camera mainCamera;

    [Header("Speed Effect")]
    public float minChromaticAberration = 0f;
    public float maxChromaticAberration = 1f;

    [Space]
    public float minBloomIntensity = 0f;
    public float maxBloomIntensity = 1f;

    [Space]
    public float minVignetteIntensity = 0.45f;
    public float maxVignetteIntensity = 0.2f;

    [Space]
    public float minSaturation = -30f;
    public float maxSaturation = 10f;

    [Space]
    public float minCameraSize = 10f;
    public float maxCameraSize = 12f;

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float speedRatio = Mathf.Clamp01((currentSpeed - minSpeedCaptured) / (maxSpeedCaptured - minSpeedCaptured));

        if (speedRatio < lastRatio)
        {
            float decayAmount = lastRatio * maxDecayPercentage * Time.deltaTime;
            speedRatio = Mathf.Max(speedRatio, lastRatio - decayAmount);
        }
        
        if (speedRatio > lastRatio)
        {
            float increaseAmount = maxIncreasePercentage * Time.deltaTime;
            speedRatio = Mathf.Min(speedRatio, lastRatio + increaseAmount);
        }

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.value = Mathf.Lerp(minChromaticAberration, maxChromaticAberration, speedRatio);
        }

        if (volume.profile.TryGet(out Bloom bloom))
        {
            bloom.intensity.value = Mathf.Lerp(minBloomIntensity, maxBloomIntensity, speedRatio);
        }

        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = Mathf.Lerp(minVignetteIntensity, maxVignetteIntensity, speedRatio);
        }

        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.saturation.value = Mathf.Lerp(minSaturation, maxSaturation, speedRatio);
        }

        if (mainCamera != null)
        {
            mainCamera.orthographicSize = Mathf.Lerp(minCameraSize, maxCameraSize, speedRatio);
        }

        lastRatio = speedRatio;

    }
}
