using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Collider2D groundTrigger; // Additional trigger component
    public bool isGrounded = false;
    private bool isDrilling = false; // Whether the player is currently drilling
    private GameObject currentGround; // The current ground object the player is in contact with

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        groundTrigger = GetComponentInChildren<Collider2D>(); // Get the trigger component from the child object
    }

    void Update()
    {
        Move();
        Jump();
        Drill();
    }

    void Move()
    {
        if (isDrilling) {
            float moveX = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0); // Move left/right using ←/→
            float moveY = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);   // Move up/down using ↑/↓

            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
            return;
        }
        float moveInput = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Can Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            // Exit drilling mode when jumping and leaving the current ground
            if (isDrilling)
            {
                isDrilling = false; 
            }
        }
    }

    void Drill()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && !isDrilling)
        {
            Debug.Log("Start Drilling");
            isDrilling = true;
            playerCollider.enabled = false; // Disable the main collider
            rb.linearVelocity = Vector2.zero; // Stop all movement
            rb.gravityScale = 0; // Disable gravity so the player stays underground
        }
    }

    // **Enable collider again when leaving the current ground**
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentGround && isDrilling)
        {
            Debug.Log("Exited ground, enabling collider");
            isDrilling = false;
            playerCollider.enabled = true; // Re-enable the main collider
            rb.gravityScale = 1; // Restore gravity
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Entered collision with ground");
            isGrounded = true;
            currentGround = collision.gameObject; // Record the ground the player is standing on
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Is not collided");
            isGrounded = false;
        }
    }
}
