using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Collider2D groundTrigger; // 额外的 Trigger 组件
    public bool isGrounded = false;
    private bool isDrilling = false; // 是否正在钻地
    private GameObject currentGround; // 当前接触的Ground对象

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        groundTrigger = GetComponentInChildren<Collider2D>(); // 获取子对象上的 Trigger
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
            float moveX = Input.GetAxis("Horizontal"); // A/D 或 ←/→ 控制左右
            float moveY = Input.GetAxis("Vertical");   // W/S 或 ↑/↓ 控制上下

            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
            return;
        }
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Can Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            // 退出钻地模式的条件：跳起并离开当前 Ground
            if (isDrilling)
            {
                isDrilling = false; 
            }
        }
    }

    void Drill()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && !isDrilling)
        {
            Debug.Log("Start Drilling");
            isDrilling = true;
            playerCollider.enabled = false; // 关闭主碰撞体
            rb.linearVelocity = Vector2.zero; // 停止所有运动
            rb.gravityScale = 0; // 停止重力，使玩家停留在地里
        }
    }

    // **当离开当前地面时，恢复碰撞**
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentGround && isDrilling)
        {
            Debug.Log("Exited ground, enabling collider");
            isDrilling = false;
            playerCollider.enabled = true; // 重新开启主碰撞体
            rb.gravityScale = 1; // 恢复重力
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Entered collision with ground");
            isGrounded = true;
            currentGround = collision.gameObject; // 记录当前站立的地面
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
