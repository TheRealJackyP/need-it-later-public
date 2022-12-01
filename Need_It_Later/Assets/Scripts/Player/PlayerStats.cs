using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Item;
using Player;
using Player.UI;
using UnityEngine;
using Weapon;

public class PlayerStats : MonoBehaviour
{
    public WeaponStats CurrentlyEquippedWeapon;
    public GameObject BuffChevronPrefab;
    public GameObject DebuffChevronPrefab;
    public GameObject BuffBar;
    public bool PauseTimers;
    public bool DebugStats;
    public List<PlayerBuff> ActiveBuffs = new();

    private void Start()
    {
        CurrentlyEquippedWeapon.Range = CurrentlyEquippedWeapon.BaseRange;
        CurrentlyEquippedWeapon.ProjectileSpeed =
            CurrentlyEquippedWeapon.BaseProjectileSpeed;
        CurrentlyEquippedWeapon.FiringRate = CurrentlyEquippedWeapon.BaseFiringRate;
        CurrentlyEquippedWeapon.ProjectileSize =
            CurrentlyEquippedWeapon.BaseProjectileSize;
        CurrentlyEquippedWeapon.Damage = CurrentlyEquippedWeapon.BaseDamage;
        CurrentlyEquippedWeapon.Spread = CurrentlyEquippedWeapon.BaseSpread;
    }

    private void Update()
    {
        if (DebugStats)
            Debug.Log(
                "Range: " +
                CurrentlyEquippedWeapon.Range +
                "\n" +
                "ProjectileSpeed: " +
                CurrentlyEquippedWeapon.ProjectileSpeed +
                "\n" +
                "FiringRate: " +
                CurrentlyEquippedWeapon.FiringRate +
                "\n" +
                "ProjectileSize: " +
                CurrentlyEquippedWeapon.ProjectileSize +
                "\n" +
                "Damage: " +
                CurrentlyEquippedWeapon.Damage +
                "\n" +
                "Spread: " +
                CurrentlyEquippedWeapon.Spread +
                "\n");
    }

    public void DoApplyBuff(ItemScriptableObject TargetItem)
    {
        var BuffsToApply = GetStatTypes(TargetItem);
        BuffsToApply.ForEach(element => ApplySpecificBuff(TargetItem, element));
    }

    public void ApplySpecificBuff(
        ItemScriptableObject TargetItem,
        BuffType BuffStatType)
    {
        if (ActiveBuffs.Any(element => element.BuffStatType == BuffStatType))
        {
            var ActiveBuff =
                ActiveBuffs.First(element => element.BuffStatType == BuffStatType);
            ActiveBuff.ElapsedDuration = 0;
            ActiveBuff.Magnitude += GetBuffMagnitude(TargetItem, BuffStatType);
            ActiveBuff.BaseDuration = Mathf.Max(
                ActiveBuff.BaseDuration,
                TargetItem.Duration);
            ActiveBuff.ApplyBuff();
        }

        else
        {
            var NewBuff = new PlayerBuff();
            NewBuff.Magnitude = GetBuffMagnitude(TargetItem, BuffStatType);
            NewBuff.BaseDuration = TargetItem.Duration;
            NewBuff.ElapsedDuration = 0f;
            NewBuff.BuffStatType = BuffStatType;
            NewBuff.TargetWeaponStats = CurrentlyEquippedWeapon;
            NewBuff.TargetBuffUI =
                CreateChevronInstance(TargetItem, BuffStatType, NewBuff)
                    .GetComponent<PlayerBuffUI>();
            NewBuff.Timer = StartCoroutine(PerformTimer(NewBuff));
            NewBuff.ApplyBuff();
            ActiveBuffs.Add(NewBuff);
        }
    }

    public void SetPauseTimers(bool value)
    {
        PauseTimers = value;
    }

    public GameObject CreateChevronInstance(
        ItemScriptableObject TargetItem,
        BuffType BuffStatType,
        PlayerBuff buffInstance)
    {
        if (GetBuffMagnitude(TargetItem, BuffStatType) > 0)
        {
            var newInstance = Instantiate(BuffChevronPrefab, BuffBar.transform);
            newInstance.GetComponent<PlayerBuffUI>()
                .Init(TargetItem, BuffStatType, buffInstance);
            SortBuffBar();
            return newInstance;
        }
        else
        {
            var newInstance = Instantiate(DebuffChevronPrefab, BuffBar.transform);
            newInstance.GetComponent<PlayerBuffUI>()
                .Init(TargetItem, BuffStatType, buffInstance);
            SortBuffBar();
            return newInstance;
        }
    }

    public void SortBuffBar()
    {
        var PlayerBuffs = BuffBar.GetComponentsInChildren<PlayerBuffUI>().ToList();
        PlayerBuffs.Sort(
            (lhs, rhs) => CalculateBuffPriority(lhs) - CalculateBuffPriority(rhs));
        PlayerBuffs.ForEach(
            element => element.GetComponent<RectTransform>()
                .SetSiblingIndex(PlayerBuffs.IndexOf(element)));
    }

    public int CalculateBuffPriority(PlayerBuffUI TargetBuff)
    {
        var BuffPriority = ConvertBuffType(TargetBuff.TargetPlayerBuff.BuffStatType);

        return TargetBuff.TargetPlayerBuff.Magnitude > 0
            ? BuffPriority - 7
            : BuffPriority;
    }

    public int ConvertBuffType(BuffType TargetType)
    {
        return TargetType switch
        {
            BuffType.Default => 0,
            BuffType.Range => 4,
            BuffType.ProjectileSpeed => 3,
            BuffType.FiringRate => 2,
            BuffType.ProjectileSize => 6,
            BuffType.Damage => 1,
            BuffType.Spread => 5,
            _ => throw new ArgumentOutOfRangeException(
                nameof(TargetType),
                TargetType,
                null)
        };
    }


    public int GetBuffMagnitude(ItemScriptableObject TargetItem, BuffType BuffStatType)
    {
        if (BuffStatType == BuffType.Range)
            return Mathf.RoundToInt(TargetItem.AddRange);
        if (BuffStatType == BuffType.ProjectileSpeed)
            return Mathf.RoundToInt(TargetItem.AddProjectileSpeed);
        if (BuffStatType == BuffType.FiringRate)
            return Mathf.RoundToInt(TargetItem.AddFireRate);
        if (BuffStatType == BuffType.ProjectileSize)
            return Mathf.RoundToInt(TargetItem.AddProjectileSize);
        if (BuffStatType == BuffType.Damage)
            return Mathf.RoundToInt(TargetItem.AddDamage);
        if (BuffStatType == BuffType.Spread)
            return Mathf.RoundToInt(TargetItem.AddAccuracy);
        throw new ArgumentOutOfRangeException(nameof(BuffStatType), BuffStatType, null);
    }

    public List<BuffType> GetStatTypes(ItemScriptableObject TargetItem)
    {
        List<BuffType> BuffTypes = new();
        if (TargetItem.AddAccuracy != 0) BuffTypes.Add(BuffType.Spread);
        if (TargetItem.AddDamage != 0) BuffTypes.Add(BuffType.Damage);
        if (TargetItem.AddRange != 0) BuffTypes.Add(BuffType.Range);
        if (TargetItem.AddFireRate != 0) BuffTypes.Add(BuffType.FiringRate);
        if (TargetItem.AddProjectileSize != 0) BuffTypes.Add(BuffType.ProjectileSize);
        if (TargetItem.AddProjectileSpeed != 0) BuffTypes.Add(BuffType.ProjectileSpeed);

        return BuffTypes;
    }

    public IEnumerator PerformTimer(PlayerBuff TargetBuff)
    {
        while (TargetBuff.ElapsedDuration <= TargetBuff.BaseDuration &&
               TargetBuff.Magnitude != 0)
        {
            if (!PauseTimers) TargetBuff.ElapsedDuration += Time.smoothDeltaTime;
            yield return null;
        }

        TargetBuff.RevertBuff();
        TargetBuff.TargetBuffUI.EndBuff(TargetBuff.BuffStatType);
        ActiveBuffs.Remove(TargetBuff);
    }
}