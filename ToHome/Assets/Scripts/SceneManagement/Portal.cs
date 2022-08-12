using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        
        [SerializeField] private int destinationSceneIndex = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals("Player")) return;
            
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (destinationSceneIndex < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(destinationSceneIndex);
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            print("Scene loaded.");
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}
