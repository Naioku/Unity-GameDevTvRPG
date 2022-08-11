using UnityEngine;

namespace Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float waypointDisplayRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointDisplayRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return transform.GetChild(i).transform.position;
        }

        public int GetNextIndex(int index)
        {
            return index + 1 == transform.childCount ? 0 : index + 1;
        }
    }
}
