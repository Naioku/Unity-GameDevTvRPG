using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int destinationSceneIndex = -1;
        [SerializeField] private Transform spawnPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals("Player")) return;
            
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
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

                return portal;
            }

            return null;
        }
    }
}
