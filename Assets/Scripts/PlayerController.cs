using System;
using UnityEngine;
using UnityEngine.Serialization;
using Math = Unity.Mathematics.Geometry.Math;

public class PlayerController : MonoBehaviour
{

    public float acceleration;
    public float breakDamping;
    public float normalDamping;
    public float speedFactor;
    public float turnSpeed;
    public float maxSpeed;
    public float minSpeed;
    // public float turnSpeed = 100.0f;
    //
    
    private Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // add less force if we are close to max speed
            speedFactor = 1.0f - (rb.linearVelocity.magnitude / maxSpeed);
            speedFactor *= speedFactor; // square for smoother effect
            rb.AddForce(transform.up * (acceleration * speedFactor), ForceMode2D.Impulse);
        }
        rb.linearDamping = normalDamping;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearDamping = breakDamping;
        }

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        if (rb.linearVelocity .magnitude < minSpeed)
        {
            rb.linearVelocity = Vector3.zero;
        }
        
        transform.Rotate(Vector3.forward * (-turnInput * turnSpeed * Time.deltaTime));
        // project onto forward direction
        rb.linearVelocity = Vector3.Project(rb.linearVelocity, transform.up);
        rb.angularVelocity = 0.0f;
        
        // if (Input.GetKey(KeyCode.W))
        // {
        //     speed += acceleration * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     speed -= acceleration * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
        // }
        // speed -= acceleration * 0.5f * Time.deltaTime; // Natural deceleration
        // speed = Mathf.Clamp(speed, 0, 20);
        // transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
