using UnityEngine;
using UnityEngine.Events;

namespace Health
{
public class Hurtbox : HealthHandler
{
    [SerializeField] private EntityHealth _parentEntityHealth;

    public override void AddHealth(float amount)
    {
        _parentEntityHealth.AddHealth(amount);
    }

    public override void AddHealthUnclamped(float amount)
    {
        _parentEntityHealth.AddHealthUnclamped(amount);
    }

    public override void TakeDamage(float amount)
    {
        _parentEntityHealth.TakeDamage(amount);
    }

    public override void DestroySelf()
    {
        _parentEntityHealth.DestroySelf();
    }
}
}