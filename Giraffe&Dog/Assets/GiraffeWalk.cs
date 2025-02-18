using UnityEngine;

public class GiraffeWalk : MonoBehaviour
{
    public Sprite[] walkSprites; 
    public Sprite pickupSprite;  // Head-up sprite for picking up
    public float frameRate = 0.2f; 
    public float speed = 3f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;
    private bool isPickingUp;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Handle Pickup (R Key)
        if (Input.GetKey(KeyCode.R))
        {
            spriteRenderer.sprite = pickupSprite;
            isPickingUp = true;
            return; // Skip movement if picking up
        }
        else
        {
            isPickingUp = false;
        }

        // Handle Movement (A/D Keys Only)
        float horizontal = 0;
        if (Input.GetKey(KeyCode.A)) horizontal = -1;
        if (Input.GetKey(KeyCode.D)) horizontal = 1;

        transform.Translate(new Vector2(horizontal, 0) * speed * Time.deltaTime);

        // Animate Walking
        if (horizontal != 0)
        {
            AnimateWalk();
            spriteRenderer.flipX = horizontal < 0;
        }
        else
        {
            spriteRenderer.sprite = walkSprites[0]; // Idle frame
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
}
