using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IngredientPickup : MonoBehaviour
{
    [SerializeField] private IngredientType ingredientType = IngredientType.Chili;
    [SerializeField] private float abilityDurationSeconds = 12f;

    private bool isCollected;
    private GameManager gameManager;
    private GameManagerTutorial tutorialManager;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        tutorialManager = Object.FindFirstObjectByType<GameManagerTutorial>();

        if (gameManager == null && tutorialManager == null)
        {
            Debug.LogError("IngredientPickup could not find any GameManager in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.TryGetComponent(out PlayerAbilityController abilityController)) return;

        abilityController.GrantAbility(ingredientType, abilityDurationSeconds);
        isCollected = true;

        if (tutorialManager != null)
        {
            switch (ingredientType)
            {
                case IngredientType.Chili:
                    tutorialManager.OnChiliCollected();
                    break;
                case IngredientType.Butter:
                    tutorialManager.OnButterCollected();
                    break;
                case IngredientType.Bread:
                    tutorialManager.OnBreadCollected();
                    break;
            }
        }
        else if (gameManager != null)
        {
            gameManager.OnIngredientEaten();
        }

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
