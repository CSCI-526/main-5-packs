using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IngredientPickup : MonoBehaviour
{
    [SerializeField] private IngredientType ingredientType = IngredientType.Chili;
    [SerializeField] private float abilityDurationSeconds = 12f;

    private bool isCollected;
    private GameManager gameManager; // NEW: A variable to hold a reference to the GameManager.

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // NEW: Add a Start method to find the GameManager when the game begins.
    private void Start()
    {
        // Find the one and only instance of the GameManager in the scene.
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("IngredientPickup could not find the GameManager in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        // Check if the object that entered the trigger is the player.
        // This line assumes your player object has the PlayerAbilityController script.
        if (!other.TryGetComponent(out PlayerAbilityController abilityController)) return;

        // Grant the ability to the player.
        abilityController.GrantAbility(ingredientType, abilityDurationSeconds);
        
        isCollected = true;

        // NEW: Notify the GameManager that an ingredient has been collected.
        if (gameManager != null)
        {
            gameManager.OnIngredientEaten();
        }

        // Deactivate the ingredient so it disappears.
        gameObject.SetActive(false);
    }

    public void Configure(IngredientType type, float durationSeconds)
    {
        ingredientType = type;
        abilityDurationSeconds = durationSeconds;
        isCollected = false;
        gameObject.SetActive(true);
    }
}