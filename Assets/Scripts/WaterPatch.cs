using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterPatch : MonoBehaviour
{
    [SerializeField] private ParticleSystem soakEffect;
    private bool soaked;
    private GameManagerTutorial tutorialManager;
    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Wall");
    }
    private void Start()
    {
        tutorialManager = FindAnyObjectByType<GameManagerTutorial>();
    }

    public bool TrySoak(PlayerAbilityController abilityController)
    {
        if (soaked || abilityController == null) return soaked;
        if (!abilityController.ConsumeAbility(IngredientType.Bread)) return false;

        SoakInternal();
        return true;
    }

    private void SoakInternal()
    {
        soaked = true;
        if (soakEffect != null)
        {
            Instantiate(soakEffect, transform.position, Quaternion.identity);
        }
                if (tutorialManager != null)
        {
            tutorialManager.OnWaterPatchCleared();
        }
        Destroy(gameObject);
    }
}
