using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro

public class GiraffeWalk : MonoBehaviour
{
    public Sprite[] walkSprites; 
    public Sprite pickupSprite;  
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
        if (Input.GetKey(KeyCode.F))
        {
            spriteRenderer.sprite = pickupSprite;
            isPickingUp = true;
        }
        else
        {
            isPickingUp = false;
        }

        if (!isPickingUp)
        {
            float horizontal = 0;
            if (Input.GetKey(KeyCode.A)) horizontal = -1;
            if (Input.GetKey(KeyCode.D)) horizontal = 1;

            transform.Translate(new Vector2(horizontal, 0) * speed * Time.deltaTime);

            if (horizontal != 0)
            {
                AnimateWalk();
                spriteRenderer.flipX = horizontal < 0;
            }
            else
            {
                spriteRenderer.sprite = walkSprites[0]; 
            }
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Food") && Input.GetKey(KeyCode.F))
        {
            CollectFood(other.gameObject);
        }
    }

    void CollectFood(GameObject food)
    {
        if (FoodCounter.Instance != null)
        {
            FoodCounter.Instance.AddFood();
        }
        if (FoodCounterLVL2.Instance != null)
        {
            FoodCounterLVL2.Instance.AddFood();
        }
        Destroy(food);
    }
}
