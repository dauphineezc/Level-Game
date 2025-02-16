using UnityEngine;

public class GiraffeWalk : MonoBehaviour
{
    public Sprite[] walkSprites; // Assign your 3 giraffe walking sprites in the Inspector
    public float frameRate = 0.2f; // Time per frame
    public float speed = 2f;       // Walking speed

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Simple horizontal movement
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Animate walking
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            spriteRenderer.sprite = walkSprites[currentFrame];
        }
    }
}
