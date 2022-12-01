using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapon;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerGather : InputPlayerAction
    {
        [Foldout("Player Gather Parameters", foldEverything = true, styled = true, readOnly = false)]
        public float GatherRange;
        public float GatherDefaultLinearSpeed;
        public float GatherDefaultRotSpeed;
        public float GatherSpeedMultiplier;
        // public UnityEvent<PlayerGather, GameObject> OnPlayerStartGathering = new();
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

        public override void DoWhileExecuting()
        {
            HandleGatherExecute();
        }

        public void HandleGatherExecute()
        {
            if (Item.Item.ActiveItems.Any())
            {
                Item.Item.ActiveItems.ForEach(InitiateGather);
            }
        }

        public void InitiateGather(Item.Item targetItem)
        {
            var displacement = targetItem.transform.position - transform.position;
            if (targetItem.GatherCoroutine != null) return;
            if(targetItem.GatherCoroutine == null && displacement.magnitude < GatherRange)
                targetItem.StartGather(this);
        }
    }
}