using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IceWall : MonoBehaviour
{
    [SerializeField] private ParticleSystem meltEffect;

    private bool melted;
    private GameManagerTutorial tutorialManager;
    private void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Wall");
    }

    private void Start()
    {
        tutorialManager = FindAnyObjectByType<GameManagerTutorial>();
    }

    public bool TryMelt(PlayerAbilityController abilityController)
    {
        if (melted || abilityController == null) return melted;
        if (!abilityController.ConsumeAbility(IngredientType.Chili)) return false;

        MeltInternal();
        return true;
    }

    private void MeltInternal()
    {
        melted = true;
        if (meltEffect != null)
        {
            Instantiate(meltEffect, transform.position, Quaternion.identity);
        }

        // tell the tutorial manager if this happens in the tutorial scene
        GameManagerTutorial tutorialManager = Object.FindFirstObjectByType<GameManagerTutorial>();
        if (tutorialManager != null)
        {
            tutorialManager.OnIceWallMelted();
        }
        //tutorial ends
        Destroy(gameObject);
    }
}
