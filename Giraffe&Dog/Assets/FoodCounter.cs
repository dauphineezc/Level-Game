using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoodCounter : MonoBehaviour
{
    public static FoodCounter Instance; // Singleton instance

    private int foodCollected = 0;
    private int maxFood = 5; 

    [Header("UI Elements")]
    public TextMeshProUGUI counterText;
    public Image appleIcon;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        maxFood = 5;
        UpdateFoodCounter();
    }

    public void AddFood()
    {
        foodCollected++;
        UpdateFoodCounter();
    }

    void UpdateFoodCounter()
    {
        if (counterText != null)
        {
            counterText.text = foodCollected + "/" + maxFood;
        }
    }
}
