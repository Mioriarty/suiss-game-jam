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
    public bool isStunned;
    public float pushBackForceOnStun = 0.5f;

    public Exhibit inventoryExhibit;
    private Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void SetColor(Color color, float time)
    {
        GetComponent<SpriteRenderer>().color = color;
        Invoke("ResetColor", time);
    }
    
    void UnstunPlayer()
    {
        isStunned = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Adult"))
        {
            isStunned = true;
            // set color to red
            
            
            // add a force away from the collision point
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 directionAway = (transform.position - collisionPoint).normalized;
            rb.AddForce(directionAway * (pushBackForceOnStun * 5.0f), ForceMode2D.Impulse);
            
            // after 2 seconds, unstun the player
            Invoke("UnstunPlayer", 1.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (isStunned)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }
        
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // add less force if we are close to max speed
            speedFactor = 1.0f - (rb.linearVelocity.magnitude / maxSpeed);
            speedFactor *= speedFactor; // square for smoother effect
            rb.AddForce(transform.up * (acceleration * speedFactor), ForceMode2D.Impulse);
            SetColor(Color.green, 0.5f);
        }
        rb.linearDamping = normalDamping;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.linearDamping = breakDamping;
        }

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        if (rb.linearVelocity.magnitude < minSpeed)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Get All exhibits on whose triggers we are
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f, LayerMask.GetMask("Exhibit"));

            if (hits.Length > 0)
            {
                // Get the closest one
                Collider2D closest = null;
                float closestDist = float.MaxValue;
                foreach (Collider2D hit in hits)
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = hit;
                    }
                }

                ExhibitController exhibitController = closest.GetComponent<ExhibitController>();

                if (exhibitController != null)
                {
                    // Switch exhibit
                    Exhibit temp = inventoryExhibit;
                    inventoryExhibit = exhibitController.exhibit;
                    exhibitController.SetExhibit(temp);
                }
            }
        }

    }

}
