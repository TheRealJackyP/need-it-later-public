using UnityEngine;

namespace Weapon
{
    public class ProjectileAnimatorMonitor : MonoBehaviour
    {
        public Animator TargetAnimator;
        public bool StartAnimationComplete = false;
        public bool FinishAnimationComplete = false;

        public void SendProjectileStartAnimationComplete()
        {
            StartAnimationComplete = true;
        }
        
        public void SendProjectileFinishAnimationComplete()
        {
            FinishAnimationComplete = true;
        }
    }
}