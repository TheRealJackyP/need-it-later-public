using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObjects/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public FireModes FireMode;
        public float Duration;
        [Tooltip("Lower numbers are rarer.")] public int Rarity;
        
        public int Quantity;
        
        public enum FireModes
        {
            None,
            Single,
            Burst,
            Auto,
            Charge
        }

        [Header("Linear Modifiers")]
        public float AddRange;
        public float AddProjectileSpeed;
        public float AddFireRate;
        public float AddProjectileSize;
        public float AddDamage;
        public float AddAccuracy;
        
        [Header("Multiplier Modifiers")]
        public float MultRange = 1;
        public float MultProjectileSpeed = 1;
        public float MultFireRate = 1;
        public float MultProjectileSize = 1;
        public float MultDamage = 1;
        public float MultAccuracy = 1;
    }
}
