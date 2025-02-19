using UnityEngine;
public class Food : MonoBehaviour
{
    void Start()
    {
        // Optional: Add a bouncing effect to the food
        gameObject.AddComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Giraffe"))
        {
            Debug.Log("Giraffe near food!");
        }
    }
}
