using UnityEngine;

namespace Core
{
    public class PersistentObjects : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectsPrefab;

        private static bool _hasSpawned;
        
        void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistentObjects();

            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            DontDestroyOnLoad(Instantiate(persistentObjectsPrefab));
        }
    }
}
