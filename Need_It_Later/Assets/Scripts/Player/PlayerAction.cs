using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public abstract class PlayerAction : MonoBehaviour
    {
        [Foldout("Base Action Parameters", foldEverything = true, styled = true, readOnly = false)]
        public PlayerActionID ActionID;
        public PlayerActionState CurrentActionState;

        public PlayerAnimatorMonitor AnimatorMonitor;

        private bool ActivationAnimationComplete;

        public Coroutine ActivationWaitInstance;
        private bool ExecuteAnimationComplete;
        private bool StopAnimationComplete;

        public bool IsActive => CurrentActionState != PlayerActionState.Inactive;
        
        [Foldout("Base Action Unity Events", foldEverything = true, styled = true, readOnly = false)]
        public UnityEvent<PlayerAction, GameObject> OnActionActivate = new();
        public UnityEvent<PlayerAction, GameObject> OnActionActivateAnimationComplete = new();
        public UnityEvent<PlayerAction, GameObject> OnActionStart = new();
        public UnityEvent<PlayerAction, GameObject> OnActionAnimationComplete = new();
        public UnityEvent<PlayerAction, GameObject> OnActionStop = new();
        public UnityEvent<PlayerAction, GameObject> OnActionStopAnimationComplete = new();
        public UnityEvent<PlayerAction, GameObject> OnActionFinish = new();

        [Foldout("Update Intervals", foldEverything = true, styled = true, readOnly = false)]
        public ActionUpdateMode UpdateMode;
        public float CustomUpdateInterval;
        
        [Foldout("Action Wait Modes", foldEverything = true, styled = true, readOnly = false)]
        public ActionEventWaitMode ActivationWaitMode;
        public ActionEventWaitMode ExecuteWaitMode;
        public ActionEventWaitMode StopWaitMode;
        public float ActivationWaitInterval;
        public float ExecuteWaitInterval;
        public float StopWaitInterval;

        

        public virtual void OnEnable()
        {
            OnActionActivate.AddListener(HandleActionActivate);
            OnActionActivateAnimationComplete.AddListener(HandleActionActivateAnimationComplete);
            OnActionStart.AddListener(HandleActionStart);
            OnActionAnimationComplete.AddListener(HandleActionAnimationComplete);
            OnActionStop.AddListener(HandleActionStop);
            OnActionStopAnimationComplete.AddListener(HandleActionStopAnimationComplete);
            OnActionFinish.AddListener(HandleActionFinish);
        }

        private void OnDisable()
        {
            OnActionActivate.RemoveListener(HandleActionActivate);
            OnActionActivateAnimationComplete.RemoveListener(HandleActionActivateAnimationComplete);
            OnActionStart.RemoveListener(HandleActionStart);
            OnActionAnimationComplete.RemoveListener(HandleActionAnimationComplete);
            OnActionStop.RemoveListener(HandleActionStop);
            OnActionStopAnimationComplete.RemoveListener(HandleActionStopAnimationComplete);
            OnActionFinish.RemoveListener(HandleActionFinish);
            if(ActivationWaitInstance != null)
                StopCoroutine(ActivationWaitInstance);
        }

        public virtual bool CanActivatePlayerAction()
        {
            return !IsActive;
        }

        public virtual bool TryActivatePlayerAction(bool forceActivation = false)
        {
            if (forceActivation || CanActivatePlayerAction())
            {
                ActivationWaitInstance = StartCoroutine(DoAction());
                return true;
            }

            return false;
        }

        public void CompleteActivateAnimation()
        {
            ActivationAnimationComplete = true;
        }

        public void CompleteActionAnimation()
        {
            ExecuteAnimationComplete = true;
        }

        public void CompleteStopAnimation()
        {
            StopAnimationComplete = true;
        }

        public virtual bool CompleteActivationWait()
        {
            return true;
        }

        public virtual bool CompleteExecuteWait()
        {
            return true;
        }

        public virtual bool CompleteStopWait()
        {
            return true;
        }

        public virtual void DoWhileActivating()
        {
        }

        public virtual void DoWhileExecuting()
        {
        }

        public virtual void DoWhileStopping()
        {
        }

        public virtual void HandleActionActivate(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionActivateAnimationComplete(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionStart(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionAnimationComplete(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionStop(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionStopAnimationComplete(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual void HandleActionFinish(PlayerAction targetAction, GameObject targetObject)
        {
        }

        public virtual IEnumerator DoAction()
        {
            // Activate Action
            CurrentActionState = PlayerActionState.Activating;
            OnActionActivate.Invoke(this, gameObject);
            if (ActivationWaitMode == ActionEventWaitMode.Animation)
            {
                ActivationAnimationComplete = false;
                while (!ActivationAnimationComplete)
                {
                    DoWhileActivating();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }

                OnActionActivateAnimationComplete.Invoke(this, gameObject);
            }
            else if (ActivationWaitMode == ActionEventWaitMode.TimeDelay)
            {
                var elapsedActivationWaitTime = 0f;
                while (elapsedActivationWaitTime < ActivationWaitInterval)
                {
                    DoWhileActivating();
                    elapsedActivationWaitTime += UpdateMode switch
                    {
                        ActionUpdateMode.Update => Time.deltaTime,
                        ActionUpdateMode.FixedUpdate => Time.fixedDeltaTime,
                        _ => CustomUpdateInterval
                    };

                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }
            else if (ActivationWaitMode == ActionEventWaitMode.Custom)
            {
                while (!CompleteActivationWait())
                {
                    DoWhileActivating();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }

            //Execute Action
            CurrentActionState = PlayerActionState.Executing;
            OnActionStart.Invoke(this, gameObject);
            if (ExecuteWaitMode == ActionEventWaitMode.Animation)
            {
                ExecuteAnimationComplete = false;
                while (!ExecuteAnimationComplete)
                {
                    DoWhileExecuting();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }

                OnActionAnimationComplete.Invoke(this, gameObject);
            }
            else if (ExecuteWaitMode == ActionEventWaitMode.TimeDelay)
            {
                var elapsedExecuteWaitTime = 0f;
                while (elapsedExecuteWaitTime < ExecuteWaitInterval)
                {
                    DoWhileExecuting();
                    elapsedExecuteWaitTime += UpdateMode switch
                    {
                        ActionUpdateMode.Update => Time.deltaTime,
                        ActionUpdateMode.FixedUpdate => Time.fixedDeltaTime,
                        _ => CustomUpdateInterval
                    };

                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }
            else if (ExecuteWaitMode == ActionEventWaitMode.Custom)
            {
                while (!CompleteExecuteWait())
                {
                    DoWhileExecuting();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }

            //Stop Action
            CurrentActionState = PlayerActionState.Stopping;
            OnActionStop.Invoke(this, gameObject);
            if (StopWaitMode == ActionEventWaitMode.Animation)
            {
                StopAnimationComplete = false;
                while (!StopAnimationComplete)
                {
                    DoWhileStopping();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }

                OnActionStopAnimationComplete.Invoke(this, gameObject);
            }
            else if (StopWaitMode == ActionEventWaitMode.TimeDelay)
            {
                var elapsedStopWaitTime = 0f;
                while (elapsedStopWaitTime < StopWaitInterval)
                {
                    DoWhileStopping();
                    elapsedStopWaitTime += UpdateMode switch
                    {
                        ActionUpdateMode.Update => Time.deltaTime,
                        ActionUpdateMode.FixedUpdate => Time.fixedDeltaTime,
                        _ => CustomUpdateInterval
                    };

                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }
            else if (StopWaitMode == ActionEventWaitMode.Custom)
            {
                while (!CompleteStopWait())
                {
                    DoWhileStopping();
                    yield return UpdateMode switch
                    {
                        ActionUpdateMode.Update => null,
                        ActionUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
                        _ => new WaitForSeconds(CustomUpdateInterval)
                    };
                }
            }

            //Finish Action
            OnActionFinish.Invoke(this, gameObject);
            CurrentActionState = PlayerActionState.Inactive;
            ActivationWaitInstance = null;
        }
    }

    public enum PlayerActionID
    {
        Idle = 0,
        Move = 1,
        Fire = 2,
        Aim = 3,
        Gather = 4
    }

    public enum ActionEventWaitMode
    {
        None = 0,
        Animation = 1,
        TimeDelay = 2,
        Custom = 3
    }

    public enum ActionUpdateMode
    {
        Update = 0,
        FixedUpdate = 1,
        Custom = 2
    }

    public enum PlayerActionState
    {
        Inactive = 0,
        Activating = 1,
        Executing = 2,
        Stopping = 3
    }
}