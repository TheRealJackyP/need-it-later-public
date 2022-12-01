using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapon;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerFire : InputPlayerAction
    {
        [Foldout("Player Fire Parameters", foldEverything = true, styled = true, readOnly = false)]
        public PlayerStats TargetPlayerStats;
        public float NextFireTime;
        public float ProjectileOffset;
        public PlayerAim PlayerAim;
        public UnityEvent<PlayerFire, GameObject> OnPlayerFired = new();
        private bool finishInput;

        public override bool CompleteExecuteWait()
        {
            return finishInput;
        }

        public override void HandleInputStarted(InputAction.CallbackContext context)
        {
            base.HandleInputStarted(context);
            finishInput = false;
            TryActivatePlayerAction();
        }

        public override void HandleInputPerformed(InputAction.CallbackContext context)
        {
        }

        public override void HandleInputStopped(InputAction.CallbackContext context)
        {
            finishInput = true;
        }

        public override void HandleActionStart(
            PlayerAction targetAction,
            GameObject targetObject)
        {
            if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Single)
            {
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Auto)
            {
                HandleAutoFireStart();
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Charge)
            {
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Burst)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleAutoFireStart()
        {
            if (NextFireTime <= 0)
            {
                NextFireTime = TargetPlayerStats.CurrentlyEquippedWeapon.FiringRate;
                FireWeapon();
            }
        }

        public virtual void FireWeapon()
        {
            NextFireTime = TargetPlayerStats.CurrentlyEquippedWeapon.FiringRate;
            var spread = Random.Range(-TargetPlayerStats.CurrentlyEquippedWeapon.Spread, TargetPlayerStats.CurrentlyEquippedWeapon.Spread);
            var offset = Quaternion.Euler(0, 0, spread) *
                         PlayerAim.PlayerAimValue.normalized *
                         ProjectileOffset;
            var rotation = Quaternion.FromToRotation(Vector3.right, offset);
            var projectileObject = Instantiate(
                TargetPlayerStats.CurrentlyEquippedWeapon.ProjectilePrefab,
                transform.position + offset,
                rotation);
            var projectile = projectileObject.GetComponent<WeaponProjectile>();
            projectile.Owner = gameObject;
            projectile.Speed = TargetPlayerStats.CurrentlyEquippedWeapon.ProjectileSpeed;
            projectile.Direction = offset.normalized;
            projectile.Range = TargetPlayerStats.CurrentlyEquippedWeapon.Range;
            projectile.Damage = TargetPlayerStats.CurrentlyEquippedWeapon.Damage;
            projectile.transform.localScale *= TargetPlayerStats.CurrentlyEquippedWeapon.ProjectileSize;
            projectile.OnProjectileHit.AddListener((_,_,_,_) => SFXHandler.Instance.PlayHitSFX());
            OnPlayerFired.Invoke(this, gameObject);
        }

        public override void DoWhileExecuting()
        {
            if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Single)
            {
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Auto)
            {
                HandleAutoFireExecute();
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Charge)
            {
            }
            else if (TargetPlayerStats.CurrentlyEquippedWeapon.FiringAction == WeaponFiringAction.Burst)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleAutoFireExecute()
        {
            NextFireTime -= Time.smoothDeltaTime;
            if (NextFireTime <= 0) FireWeapon();
        }
    }
}