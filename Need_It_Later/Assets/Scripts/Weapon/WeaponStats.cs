using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Create New Weapon Stats", order = 0)]
    public class WeaponStats : ScriptableObject
    {
        public float BaseRange;
        public float BaseProjectileSpeed;
        public float BaseFiringRate;
        public float BaseProjectileSize;
        public float BaseDamage;
        public float BaseSpread;
        [NonSerialized] public float Range;
        [NonSerialized] public float ProjectileSpeed;
        [NonSerialized] public float FiringRate;
        [NonSerialized] public float ProjectileSize;
        [NonSerialized] public float Damage;
        [NonSerialized] public float Spread;
        public WeaponFiringAction FiringAction;
        public GameObject ProjectilePrefab;
    }

    public enum WeaponFiringAction
    {
        Single = 0,
        Auto = 1,
        Charge = 2,
        Burst = 3,
    }
}