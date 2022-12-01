using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAim : InputPlayerAction
    {
        [Foldout("Player Aim Parameters", foldEverything = true, styled = true, readOnly = false)]
        public Vector2 PlayerAimValue = Vector2.right;
        public Transform AimReticule;
        public Camera TargetCamera;
        public InputAction TargetAction;

        private bool finishInput;

        public override bool CompleteExecuteWait()
        {
            return false;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            TryActivatePlayerAction();
        }

        public override void DoWhileExecuting()
        {
            var mouseValue = TargetAction.ReadValue<Vector2>();
            var worldPos = TargetCamera.ScreenToWorldPoint(mouseValue);
            AimReticule.position = new Vector3(worldPos.x, worldPos.y, 0);
            PlayerAimValue = (AimReticule.position - transform.position).normalized;
        }

        // public override void HandleInputStarted(InputAction.CallbackContext context)
        // {
        //     TargetAction = context.action;
        // }

        public override void HandleInputPerformed(InputAction.CallbackContext context)
        {
            TargetAction = context.action;
        }
    }
}