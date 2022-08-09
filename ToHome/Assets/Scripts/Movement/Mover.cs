using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveToPoint();
            }
        }

        private void MoveToPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                GetComponent<NavMeshAgent>().destination = hit.point;
            }
        }
    }
}
