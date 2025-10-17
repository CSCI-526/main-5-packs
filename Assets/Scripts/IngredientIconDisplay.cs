using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class IngredientIconDisplay : MonoBehaviour
{
    [SerializeField] private IngredientType ingredientType = IngredientType.Chili;
    [SerializeField] private Image targetImage;
    [SerializeField] private float maxDimension = 70f;

    private void Reset()
    {
        targetImage = GetComponent<Image>();
        ApplyVisuals();
    }

    private void OnEnable()
    {
        ApplyVisuals();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyVisuals();
    }
#endif

    private void ApplyVisuals()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        if (targetImage == null) return;

        Sprite sprite = IngredientVisualFactory.GetSprite(ingredientType);
        if (sprite != null)
        {
            targetImage.sprite = sprite;
            targetImage.color = Color.white;
            targetImage.type = Image.Type.Simple;
            targetImage.preserveAspect = true;
        }

        if (maxDimension <= 0f) return;

        RectTransform rectTransform = targetImage.rectTransform;
        Vector3 scale = IngredientVisualFactory.GetScale(ingredientType, 1f);
        float largestAxis = Mathf.Max(scale.x, scale.y);
        if (largestAxis <= 0f) return;

        float width = (scale.x / largestAxis) * maxDimension;
        float height = (scale.y / largestAxis) * maxDimension;
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
