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
    private int foodCollected = 0;
    private int maxFood = 5;
    public TextMeshProUGUI counterText;
    public Image appleIcon;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find UI elements in the scene
        counterText = GameObject.Find("CounterText").GetComponent<TextMeshProUGUI>();
        appleIcon = GameObject.Find("AppleIcon").GetComponent<Image>();

        UpdateFoodCounter();
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
        if (other.CompareTag("Food"))
        {
            Debug.Log("Giraffe is near food.");
            if (Input.GetKey(KeyCode.F))
            {
                Debug.Log("F key pressed, picking up food.");
                CollectFood(other.gameObject);
            }
        }
    }

    void CollectFood(GameObject food)
    {
        if (foodCollected < maxFood)
        {
            foodCollected++;
            Destroy(food);
            UpdateFoodCounter();
        }
    }

    void UpdateFoodCounter()
    {
        if (counterText != null)
        {
            counterText.text = foodCollected + "/" + maxFood;
        }
    }
}
