using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IceWall : MonoBehaviour
{
    [SerializeField] private ParticleSystem meltEffect;

    private bool melted;

    private void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Wall");
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
        Destroy(gameObject);
    }
}
