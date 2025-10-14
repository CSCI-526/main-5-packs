using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Chili,
    Butter
}

public class PlayerAbilityController : MonoBehaviour
{
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

    private readonly Dictionary<IngredientType, ActiveAbility> activeAbilities = new();
    private static readonly List<IngredientType> expiredBuffer = new();

    private int stickyZoneDepth;

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
            activeAbilities.Remove(expiredBuffer[i]);
        }
    }

    public void GrantAbility(IngredientType type, float durationSeconds)
    {
        activeAbilities[type] = new ActiveAbility(type, durationSeconds);
    }

    public bool ConsumeAbility(IngredientType type)
    {
        if (!HasAbility(type)) return false;

        activeAbilities.Remove(type);
        return true;
    }

    public bool HasAbility(IngredientType type)
    {
        return activeAbilities.ContainsKey(type);
    }

    public void EnterStickyZone()
    {
        stickyZoneDepth++;
    }

    public void ExitStickyZone()
    {
        stickyZoneDepth = Mathf.Max(0, stickyZoneDepth - 1);
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

        return multiplier;
    }

    private bool IsInsideStickyZone()
    {
        return stickyZoneDepth > 0;
    }
}
