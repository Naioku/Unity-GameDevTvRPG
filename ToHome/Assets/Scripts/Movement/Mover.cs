using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            GetComponent<NavMeshAgent>().destination = target.position;
        }
    }
}
