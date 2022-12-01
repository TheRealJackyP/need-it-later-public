using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Health;
using UnityEngine;
using UnityEngine.Events;

public class PlayerXP : MonoBehaviour
{
    public float BaseXP;
    public float CurrentXP;
    public int CurrentLevel;

    public float XPMultiplier;
    public float BaseKillXP;
    public float BaseRoundXP;

    public UnityEvent<PlayerXP, int> OnPlayerLevelUp = new();

    public CombatRoundManager TargetRoundManger;
    public PlayerInventoryHandler TargetInventoryHandler;
    public EntityHealth PlayerHealth;

    public void LevelUp()
    {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 1, 99);
        if (CurrentLevel < 99)
        {
            CurrentXP = Mathf.Clamp(CurrentXP - BaseXP, 0, BaseXP);
            BaseXP *= Mathf.Pow(XPMultiplier,2);
            UpdateHealth();
            OnPlayerLevelUp.Invoke(this, CurrentLevel);
        }
        else
        {
            CurrentXP = BaseXP;
            PlayerHealth.CurrentHealth = PlayerHealth.BaseHealth;
        }

    }

    public void AddXP(float amount)
    {
        CurrentXP += amount;
        if (CurrentXP >= BaseXP && CurrentLevel < 99)
        {
            LevelUp();
        }
    }

    public void AwardKillXP(EntityHealth targetHealth, GameObject targetObject)
    {
        if (CurrentLevel < 99)
        {
            AddXP(BaseKillXP + (TargetRoundManger.ElapsedRounds * XPMultiplier));
        }
    }

    public void AwardEndOfRoundXP(CombatRoundManager targetRoundManager)
    {
        if (CurrentLevel < 99)
        {
            var itemCount = TargetInventoryHandler.PlayerItemQuantities.Values.Aggregate(0,
                ((total, next) => total + next));
            AddXP(BaseRoundXP * Mathf.Pow(XPMultiplier, TargetRoundManger.ElapsedRounds) * itemCount);
        }
    }

    public void UpdateHealth()
    {
        PlayerHealth.BaseHealth += 20;
        PlayerHealth.CurrentHealth = PlayerHealth.BaseHealth;
    }
}
