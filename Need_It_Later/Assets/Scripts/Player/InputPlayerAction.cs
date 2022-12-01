using System;
using UnityEngine.InputSystem;

namespace Player
{
    public abstract class InputPlayerAction : PlayerAction
    {
        public virtual void HandleInputAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                HandleInputStarted(context);
            }
            else if (context.phase == InputActionPhase.Performed)
            {
                HandleInputPerformed(context);
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                HandleInputStopped(context);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void HandleInputStarted(InputAction.CallbackContext context)
        {
            
        }
        
        public virtual void HandleInputPerformed(InputAction.CallbackContext context)
        {
            
        }
        
        public virtual void HandleInputStopped(InputAction.CallbackContext context)
        {
            
        }
        
    }
}