using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMove : InputPlayerAction
    {
        [Foldout("Player Move Parameters", foldEverything = true, styled = true, readOnly = false)]
        public float Acceleration;
        public float MaxSpeed;
        public Rigidbody2D PlayerRigidbody;
        public Vector2 PlayerInputValue;
        public Collider2D PlayerCollider;
        public float PlayerStopDistance;
        public LayerMask PlayerStopMask;

        private bool finishInput;

        public override bool CompleteExecuteWait()
        {
            return finishInput && Vector2.Distance(PlayerRigidbody.velocity, Vector2.zero) <= .1f;
        }

        public override void HandleInputStarted(InputAction.CallbackContext context)
        {
            base.HandleInputStarted(context);
            finishInput = false;
            TryActivatePlayerAction();
        }

        public override void HandleInputPerformed(InputAction.CallbackContext context)
        {
            PlayerInputValue = context.ReadValue<Vector2>();
        }

        public override void HandleInputStopped(InputAction.CallbackContext context)
        {
            PlayerInputValue = Vector2.zero;
            finishInput = true;
        }

        public override void DoWhileExecuting()
        {
            if (Mathf.Abs(PlayerInputValue.x) > 0.01f)
                AnimatorMonitor.TargetAnimator.SetBool("IsFacingLeft", PlayerInputValue.x < 0);
            AnimatorMonitor.TargetAnimator.SetFloat("XSpeed", Mathf.Abs(PlayerInputValue.x));
            AnimatorMonitor.TargetAnimator.SetFloat("YSpeed", Mathf.Abs(PlayerInputValue.y));
            var currentVelocity = PlayerRigidbody.velocity;
            var nextVelocity = Vector2.Lerp(currentVelocity, PlayerInputValue * MaxSpeed, Acceleration *
                UpdateMode switch
                {
                    ActionUpdateMode.Update => Time.deltaTime,
                    ActionUpdateMode.FixedUpdate => Time.fixedDeltaTime,
                    ActionUpdateMode.Custom => CustomUpdateInterval,
                    _ => throw new ArgumentOutOfRangeException()
                });

            List<RaycastHit2D> hits = new();
            if (PlayerCollider.Cast(
                    nextVelocity.normalized,
                    new ContactFilter2D()
                    {
                        layerMask = PlayerStopMask,
                        useLayerMask = true
                    },
                    hits,
                    PlayerStopDistance, false) >
                0)
            {
               nextVelocity = hits.Aggregate<RaycastHit2D, Vector3>(nextVelocity, AdjustForCollision);
            }

            PlayerRigidbody.velocity = nextVelocity;
        }

        public Vector3 AdjustForCollision(Vector3 originalVelocity, RaycastHit2D hit)
        {
            var closePoint = hit.collider.ClosestPoint(hit.point);
            var selfClosePoint = PlayerCollider.ClosestPoint(closePoint);
            var displacement = closePoint - (Vector2)transform.position;
            if (Vector2.Distance(closePoint, selfClosePoint) <= PlayerStopDistance)
            {
                var projection = Vector3.Project(originalVelocity, displacement);
                return originalVelocity - projection;
            }

            return originalVelocity;
        }

        public override void HandleActionStop(PlayerAction targetAction, GameObject targetObject)
        {
            PlayerRigidbody.velocity = Vector2.zero;
            AnimatorMonitor.TargetAnimator.SetFloat("XSpeed", Mathf.Abs(PlayerInputValue.x));
            AnimatorMonitor.TargetAnimator.SetFloat("YSpeed", Mathf.Abs(PlayerInputValue.y));
        }
    }
}