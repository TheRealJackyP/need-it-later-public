using System;
using Health;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Weapon
{
    public class WeaponProjectile : MonoBehaviour
    {
        [Foldout("Base Projectile Parameters", foldEverything = true, styled = true, readOnly = false)]
        public GameObject Owner;
        public float Speed;
        public float Range;
        public float Damage;
        
        [Foldout("Projectile References", foldEverything = true, styled = true, readOnly = false)]
        public Rigidbody2D SelfRigidBody;
        public ProjectileAnimatorMonitor AnimatorMonitor;

        [Foldout("Projectile Runtime Data", foldEverything = true, styled = true, readOnly = false)]
        public float RangeTraveled;
        public Vector2 Direction;
        private bool StartCompleteRaised;
        private bool AwaitingFinishAnimation; 

        [Foldout("Base Projectile Unity Events", foldEverything = true, styled = true, readOnly = false)]
        public UnityEvent<WeaponProjectile, GameObject>
            OnProjectileStart = new();
        
        public UnityEvent<WeaponProjectile, GameObject>
            OnProjectileStartAnimationComplete = new();
        
        public UnityEvent<WeaponProjectile, GameObject, GameObject, float>
            OnProjectileHit = new();
        
        public UnityEvent<WeaponProjectile, GameObject, GameObject>
            OnProjectileCollide = new(); 
        
        public UnityEvent<WeaponProjectile, GameObject>
            OnProjectileEnd = new();
        
        public UnityEvent<WeaponProjectile, GameObject>
            OnProjectileEndAnimationComplete = new();

        public virtual void OnEnable()
        {
            OnProjectileStart.Invoke(this, gameObject);
        }

        public virtual void FixedUpdate()
        {
            if (AwaitingFinishAnimation)
            {
                if(AnimatorMonitor.FinishAnimationComplete)
                    OnProjectileEndAnimationComplete.Invoke(this, gameObject);
                return;
            }
            if (!StartCompleteRaised && AnimatorMonitor.StartAnimationComplete)
                OnProjectileStartAnimationComplete.Invoke(this, gameObject);
            
            RangeTraveled += SelfRigidBody.velocity.magnitude * Time.fixedDeltaTime;
            if (RangeTraveled >= Range)
            {
                OnProjectileEnd.Invoke(this, gameObject);
                AwaitingFinishAnimation = true;
                return;
            }
            SelfRigidBody.velocity = Speed * Direction;
        }

        public virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                OnProjectileCollide.Invoke(this, gameObject, col.gameObject);
            }
            else if (col.collider.TryGetComponent<HealthHandler>(out var TargetHealth))
            {
                TargetHealth.TakeDamage(Damage);
                OnProjectileHit.Invoke(this, gameObject, TargetHealth.gameObject, Damage);
            }
        }

        public virtual void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}