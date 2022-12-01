
using UnityEngine;

namespace Health
{
public abstract class HealthHandler : MonoBehaviour
{

    public abstract void AddHealth(float amount);

    public abstract void AddHealthUnclamped(float amount);

    public abstract  void TakeDamage(float amount);

    public abstract void DestroySelf();
}
}
