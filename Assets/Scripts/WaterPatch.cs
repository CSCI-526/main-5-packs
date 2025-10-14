using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterPatch : MonoBehaviour
{
    [SerializeField] private ParticleSystem soakEffect;
    private bool soaked;

    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Wall");
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
        Destroy(gameObject);
    }
}
