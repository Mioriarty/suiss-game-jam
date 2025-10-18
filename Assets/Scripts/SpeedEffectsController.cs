using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class SpeedEffectsController : MonoBehaviour
{
    public float maxSpeedCaptured = 10f;

    public Volume volume;

    [Header("Speed Effect")]
    public float minChromaticAberration = 0f;
    public float maxChromaticAberration = 1f;

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeedCaptured);

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.value = Mathf.Lerp(minChromaticAberration, maxChromaticAberration, speedRatio);
        }

    }
}
