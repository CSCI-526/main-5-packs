using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerColorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private Coroutine activePulse;

    private readonly List<Color> activeColors = new();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }

    public void PlayPowerAnimation(IngredientType type)
    {
        Color powerColor = GetPowerColor(type);

        if (!activeColors.Contains(powerColor))
        {
            activeColors.Add(powerColor);
        }

        if (activePulse != null)
        {
            StopCoroutine(activePulse);
        }

        activePulse = StartCoroutine(CycleColors());
    }

    public void StopPowerAnimation(IngredientType? type = null)
    {
        if (type != null)
        {
            Color c = GetPowerColor(type.Value);
            activeColors.Remove(c);
        }

        if (activeColors.Count == 0)
        {
            if (activePulse != null)
            {
                StopCoroutine(activePulse);
                activePulse = null;
            }

            spriteRenderer.color = baseColor;
        }
    }
    public void StopAllPowerAnimationsInstant()
    {
        if (activePulse != null)
        {
            StopCoroutine(activePulse);
            activePulse = null;
        }

        activeColors.Clear();
        spriteRenderer.color = baseColor;
    }


    private IEnumerator CycleColors()
    {
        int index = 0;

        while (true)
        {
            if (activeColors.Count == 0)
            {
                spriteRenderer.color = baseColor;
                yield break;
            }

            Color currentColor = activeColors[index % activeColors.Count];
            yield return PulseColor(currentColor);

            index++;
        }
    }

    private IEnumerator PulseColor(Color powerColor)
    {
        float pulseSpeed = 3f;
        float minAlpha = 0.6f;
        float maxAlpha = 1f;

        float cycleTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < cycleTime)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float brightness = Mathf.Lerp(minAlpha, maxAlpha, t);

            spriteRenderer.color = new Color(
                powerColor.r * brightness,
                powerColor.g * brightness,
                powerColor.b * brightness,
                1f
            );

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private Color GetPowerColor(IngredientType type)
    {
        return type switch
        {
            IngredientType.Chili => new Color(1f, 0.35f, 0f, 1f),
            IngredientType.Butter => new Color(1f, 0.9f, 0.3f, 1f), 
            IngredientType.Bread => new Color(0.35f, 0.2f, 0.05f, 1f), 
            IngredientType.Garlic => new Color(0.9f, 0.92f, 0.8f, 1f),
            IngredientType.Chocolate => new Color(0.5f, 0.32f, 0.2f, 1f),
            _ => baseColor
        };
    }
}
