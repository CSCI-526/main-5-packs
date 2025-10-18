using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Chili,
    Butter,
    Bread,
    Garlic,
    Chocolate
}

public class PlayerAbilityController : MonoBehaviour
{
    private PlayerColorController colorController;
    private sealed class ActiveAbility
    {
        public readonly IngredientType Type;
        public readonly bool HasUnlimitedDuration;
        public float RemainingTime;

        public ActiveAbility(IngredientType type, float durationSeconds)
        {
            Type = type;
            HasUnlimitedDuration = durationSeconds <= 0f;
            RemainingTime = Mathf.Max(0f, durationSeconds);
        }

        public void Tick(float deltaTime)
        {
            if (HasUnlimitedDuration) return;
            RemainingTime = Mathf.Max(0f, RemainingTime - deltaTime);
        }

        public bool IsExpired => !HasUnlimitedDuration && RemainingTime <= 0f;
    }

    [Header("Ability Balancing")]
    [SerializeField] private float butterSpeedMultiplier = 1.4f;
    [SerializeField] private float stickySlowMultiplier = 0.35f;
    [SerializeField] private float chocolateSpeedMultiplier = 1.25f;

    private readonly Dictionary<IngredientType, ActiveAbility> activeAbilities = new();
    private static readonly List<IngredientType> expiredBuffer = new();

    private int stickyZoneDepth;

    private void Awake()
    {
        colorController = GetComponent<PlayerColorController>();
    }


    private void Update()
    {
        if (activeAbilities.Count == 0) return;

        expiredBuffer.Clear();
        foreach (KeyValuePair<IngredientType, ActiveAbility> entry in activeAbilities)
        {
            entry.Value.Tick(Time.deltaTime);
            if (entry.Value.IsExpired)
            {
                expiredBuffer.Add(entry.Key);
            }
        }

        for (int i = 0; i < expiredBuffer.Count; i++)
        {
            IngredientType expiredType = expiredBuffer[i];
            activeAbilities.Remove(expiredType);

            if (expiredType == IngredientType.Butter)
            {
                if (!IsInsideStickyZone() && colorController != null)
                    colorController.StopPowerAnimation(expiredType);
            }
            else
            {
                if (colorController != null)
                    colorController.StopPowerAnimation(expiredType);
            }
        }

        if (activeAbilities.Count == 0 && colorController != null && !IsInsideStickyZone())
        {
            colorController.StopPowerAnimation();
        }
    }


    public void GrantAbility(IngredientType type, float durationSeconds)
    {
        activeAbilities[type] = new ActiveAbility(type, durationSeconds);

        if (colorController != null)
        {
            colorController.PlayPowerAnimation(type);
        }
    }

    public bool ConsumeAbility(IngredientType type)
    {
        if (!HasAbility(type)) return false;

        activeAbilities.Remove(type);

        if (colorController != null)
        {
            colorController.StopPowerAnimation(type);
        }

        return true;
    }

    public bool TryConsumeAnyAbility(out IngredientType consumedType)
    {
        if (activeAbilities.Count == 0)
        {
            consumedType = default;
            return false;
        }

        using Dictionary<IngredientType, ActiveAbility>.Enumerator enumerator = activeAbilities.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            consumedType = default;
            return false;
        }

        consumedType = enumerator.Current.Key;
        ConsumeAbility(consumedType);
        return true;
    }

    public bool HasAbility(IngredientType type)
    {
        return activeAbilities.ContainsKey(type);
    }

    public void EnterStickyZone()
    {
        stickyZoneDepth++;
         if (HasAbility(IngredientType.Butter) && colorController != null)
        {
            colorController.PlayPowerAnimation(IngredientType.Butter);
        }
    }

    public void ExitStickyZone()
    {
        stickyZoneDepth = Mathf.Max(0, stickyZoneDepth - 1);

        if (stickyZoneDepth == 0)
        {
            if (!HasAbility(IngredientType.Butter) && colorController != null)
            {
                colorController.StopAllPowerAnimationsInstant();
            }
        }
    }


    public float GetMoveSpeedMultiplier()
    {
        float multiplier = 1f;

        if (IsInsideStickyZone() && !HasAbility(IngredientType.Butter))
        {
            multiplier *= stickySlowMultiplier;
        }

        if (HasAbility(IngredientType.Butter))
        {
            multiplier *= butterSpeedMultiplier;
        }

        if (HasAbility(IngredientType.Chocolate))
        {
            multiplier *= chocolateSpeedMultiplier;
        }

        return multiplier;
    }

    private bool IsInsideStickyZone()
    {
        return stickyZoneDepth > 0;
    }
}
