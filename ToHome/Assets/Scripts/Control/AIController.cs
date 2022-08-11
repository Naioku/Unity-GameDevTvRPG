using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        
        void Update()
        {
            if (ShouldChasePlayer())
            {
                print(name + " should chase.");
            }
        }

        private bool ShouldChasePlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }
    }
}
