using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IngredientPickup : MonoBehaviour
{
    [SerializeField] private IngredientType ingredientType = IngredientType.Chili;
    [SerializeField] private float abilityDurationSeconds = 12f;

    private bool isCollected;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.TryGetComponent(out PlayerAbilityController abilityController)) return;

        abilityController.GrantAbility(ingredientType, abilityDurationSeconds);
        isCollected = true;
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
