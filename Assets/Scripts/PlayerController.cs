using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public Image inventoryImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateInventoryUI();
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

    void updateInventoryUI()
    {
        if (inventoryExhibit != null)
        {
            inventoryImage.sprite = inventoryExhibit.Image;
        }
        else
        {
            inventoryImage.sprite = null;
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

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
            (Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame))
        {
            // add less force if we are close to max speed
            speedFactor = 1.0f - (rb.linearVelocity.magnitude / maxSpeed);
            speedFactor *= speedFactor; // square for smoother effect
            rb.AddForce(transform.up * (acceleration * speedFactor), ForceMode2D.Impulse);
            SetColor(Color.green, 0.5f);
        }

        rb.linearDamping = normalDamping;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (Gamepad.current != null && Gamepad.current.leftTrigger.isPressed))
        {
            rb.linearDamping = breakDamping;
        }

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        if (rb.linearVelocity.magnitude < minSpeed)
        {
            rb.linearVelocity = Vector3.zero;
        }

        transform.Rotate(Vector3.forward * (-turnInput * turnSpeed * Time.deltaTime));
        rb.linearVelocity = Vector3.Project(rb.linearVelocity, transform.up);
        rb.angularVelocity = 0.0f;


        if (Input.GetKeyDown(KeyCode.Space) || (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
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
                    // Update inventory UI
                    updateInventoryUI();
                }
            }
        }
    }
}