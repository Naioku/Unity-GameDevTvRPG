using System.Collections;
using Saving;
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
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float waitBetweenFadesTime = 0.5f;

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
            
            var fader = FindObjectOfType<Fader>();
            var savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(destinationSceneIndex);
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            savingWrapper.Save();

            yield return new WaitForSecondsRealtime(waitBetweenFadesTime);
            yield return fader.FadeIn(fadeInTime);

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
