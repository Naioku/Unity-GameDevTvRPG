using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int destinationSceneIndex = -1;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals("Player")) return;
            
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(destinationSceneIndex);
            print("Scene loaded.");
            Destroy(gameObject);
        }
    }
}
