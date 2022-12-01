using System;
using Player.UI;
using UnityEngine;
using Weapon;

namespace Player
{
    public class PlayerBuff
    {
        public float BaseDuration;

        public BuffType BuffStatType;
        public float ElapsedDuration;
        private int _magnitude;
        public PlayerBuffUI TargetBuffUI;
        public WeaponStats TargetWeaponStats;
        public Coroutine Timer;

        public float Multiplier
        {
            get
            {
                if (BuffStatType == BuffType.Range) return .5f;
                if (BuffStatType == BuffType.ProjectileSpeed) return .5f;
                if (BuffStatType == BuffType.FiringRate) return .95f;
                if (BuffStatType == BuffType.ProjectileSize) return .05f;
                if (BuffStatType == BuffType.Damage) return 1f;
                if (BuffStatType == BuffType.Spread) return -1f;

                return 0;
            }
        }

        public int Magnitude
        {
            get => _magnitude;
            set => _magnitude = Mathf.Clamp(value, -10, 10);
        }

        public void ApplyBuff()
        {
            switch (BuffStatType)
            {
                case BuffType.Range:
                    TargetWeaponStats.Range = TargetWeaponStats.BaseRange +
                                              Magnitude * Multiplier;
                    break;
                case BuffType.ProjectileSpeed:
                    TargetWeaponStats.ProjectileSpeed =
                        TargetWeaponStats.BaseProjectileSpeed + Magnitude * Multiplier;
                    TargetWeaponStats.ProjectileSpeed= Mathf.Clamp(TargetWeaponStats.ProjectileSpeed, 5f, 100f);
                    break;
                case BuffType.FiringRate:
                    TargetWeaponStats.FiringRate = TargetWeaponStats.BaseFiringRate * Mathf.Pow(Multiplier, Magnitude);
                    TargetWeaponStats.FiringRate = Mathf.Clamp(TargetWeaponStats.FiringRate, .0001f, 100f);
                    break;
                case BuffType.ProjectileSize:
                    TargetWeaponStats.ProjectileSize =
                        TargetWeaponStats.BaseProjectileSize + Magnitude * Multiplier;
                    TargetWeaponStats.ProjectileSize = Mathf.Clamp(
                        TargetWeaponStats.ProjectileSize,
                        .5f,
                        100f);
                    break;
                case BuffType.Damage:
                    TargetWeaponStats.Damage = TargetWeaponStats.BaseDamage +
                                               Magnitude * Multiplier;
                    TargetWeaponStats.Damage = Mathf.Clamp(
                        TargetWeaponStats.Damage,
                        1,
                        Mathf.Infinity);
                    break;
                case BuffType.Spread:
                    TargetWeaponStats.Spread = TargetWeaponStats.BaseSpread +
                                               Magnitude * Multiplier;
                    TargetWeaponStats.Spread = Mathf.Clamp(
                        TargetWeaponStats.Spread,
                        0,
                        180);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RevertBuff()
        {
            TargetWeaponStats.Range = TargetWeaponStats.BaseRange;
            TargetWeaponStats.ProjectileSpeed = TargetWeaponStats.BaseProjectileSpeed;
            TargetWeaponStats.FiringRate = TargetWeaponStats.BaseFiringRate;
            TargetWeaponStats.ProjectileSize = TargetWeaponStats.BaseProjectileSize;
            TargetWeaponStats.Damage = TargetWeaponStats.BaseDamage;
            TargetWeaponStats.Spread = TargetWeaponStats.BaseSpread;
        }
    }


    public enum BuffType
    {
        Default = 0,
        Range = 1,
        ProjectileSpeed = 2,
        FiringRate = 3,
        ProjectileSize = 4,
        Damage = 5,
        Spread = 6
    }
}