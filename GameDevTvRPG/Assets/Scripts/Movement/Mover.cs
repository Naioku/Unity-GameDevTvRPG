using Attributes;
using Core;
using Saving;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class Mover : MonoBehaviour, IAction, ISavable
    {
        private NavMeshAgent _navMeshAgent;
        private Health _health;
        
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void CancelAction()
        {
            _navMeshAgent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3) state).ToVector();
            _navMeshAgent.enabled = true;
        }

        private void UpdateAnimator()
        {
            Vector3 globalVelocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat(ForwardSpeed, speed);
        }
    }
}
