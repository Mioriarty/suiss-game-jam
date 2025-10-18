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
    public float pushBackForceOnStun = 0.5f;

    public Exhibit inventoryExhibit;
    private Rigidbody2D rb;
    public Image inventoryImage;
    private float remainingStunTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateInventoryUI();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        remainingStunTime = 0.5f;
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 directionAway = (transform.position - collisionPoint).normalized;
        rb.AddForce(directionAway * (pushBackForceOnStun * 5.0f), ForceMode2D.Impulse);
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
        if (remainingStunTime > 0)
        {
            remainingStunTime -= Time.deltaTime;
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