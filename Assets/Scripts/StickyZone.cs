using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StickyZone : MonoBehaviour
{
    private GameManagerTutorial tutorialManager;
    private void Awake()
    {
        ConfigureCollider();
    }

    private void Start()
    {
        tutorialManager = FindAnyObjectByType<GameManagerTutorial>();
    }

    private void Reset()
    {
        ConfigureCollider();
    }

    private void ConfigureCollider()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        box.size = Vector2.one;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerAbilityController abilityController)) return;
        abilityController.EnterStickyZone();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerAbilityController abilityController)) return;
        abilityController.ExitStickyZone();
        GameManagerTutorial tutorialManager = Object.FindFirstObjectByType<GameManagerTutorial>();
        if (tutorialManager != null)
        {
            tutorialManager.OnStickyZonePassed();
        }
    }
}
