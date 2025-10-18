using UnityEngine;
using UnityEngine.Animations;

public class PlayerSpriteAnimator : MonoBehaviour
{
    private Rigidbody2D parentRb;

    public float maxRotationAngle = 15f;
    public float maxSpeedForMaxRotation = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentRb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = parentRb.linearVelocity;

        float rotationZ = Mathf.Clamp(velocity.x / maxSpeedForMaxRotation * maxRotationAngle, -maxRotationAngle, maxRotationAngle);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if(velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
