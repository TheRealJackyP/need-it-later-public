using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class NavAgentFix : MonoBehaviour
    {
        private void Start()	{
            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
}
