using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Animation Settings")]
    [SerializeField] private Sprite[] walkSprites;
    public float frameRate = 0.2f; // Speed of animation

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private bool isGrounded = false;
    private bool isDrilling = false;
    private GameObject currentGround;
    private int currentFrame;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isDrilling)
        {
            Move();
            Jump();
        }

        Drill();
        PickUpFood();
    }

    void Move()
    {
        float moveInput = 0;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            AnimateWalk();
            spriteRenderer.flipX = moveInput < 0; // Flip sprite for left movement
        }
        else
        {
            spriteRenderer.sprite = walkSprites[0]; // Idle sprite
        }
    }

    void AnimateWalk()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            spriteRenderer.sprite = walkSprites[currentFrame];
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Debug.Log("Dog Jumping");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void Drill()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && !isDrilling)
        {
            Debug.Log("Start Drilling");
            isDrilling = true;
            playerCollider.enabled = false;  // Disable the main collider
            rb.gravityScale = 0;  // Remove gravity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -3f); // Move downward slowly
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && isDrilling)
        {
            Debug.Log("Stop Drilling");
            isDrilling = false;
            playerCollider.enabled = true;  // Re-enable the collider
            rb.gravityScale = 2.5f;  // Restore gravity
            rb.linearVelocity = Vector2.zero; // Stop downward movement
        }
    }

    void PickUpFood()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Food"))
                {
                    Debug.Log("Dog picked up food!");
                    Destroy(collider.gameObject);

                    if (FoodCounter.Instance != null)
                    {
                        FoodCounter.Instance.AddFood();
                    }
                    return;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Dog Landed on ground");
            isGrounded = true;
            currentGround = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Dog Left ground");
            isGrounded = false;
        }
    }
}
