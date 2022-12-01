using UnityEngine;
using UnityEngine.Events;

namespace Health
{
    public class EntityHealth : HealthHandler
    {
        [Foldout("Entity Health Parameters", foldEverything = true, styled = true, readOnly = false)]
        public float CurrentHealth;
        public float BaseHealth;

        [Foldout("Entity Health Basic Events", foldEverything = true, styled = true, readOnly = false)]
        public UnityEvent<GameObject, float> OnTakeDamage = new();
        public UnityEvent<GameObject, float> OnGainHealth = new();
        public UnityEvent<EntityHealth, GameObject> OnEntityDie = new();

        public override void AddHealth(float amount)
        {
            var originalHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, BaseHealth);
            OnGainHealth.Invoke(gameObject, CurrentHealth - originalHealth);
        }

        public override void AddHealthUnclamped(float amount)
        {
            CurrentHealth += amount;
            OnGainHealth.Invoke(gameObject, amount);
        }

        public override void TakeDamage(float amount)
        {
            var originalHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, BaseHealth);
            OnTakeDamage.Invoke(gameObject, originalHealth - CurrentHealth);
            if (CurrentHealth <= 0)
            {
                OnEntityDie.Invoke(this, gameObject);
            }
        }

        public override void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}