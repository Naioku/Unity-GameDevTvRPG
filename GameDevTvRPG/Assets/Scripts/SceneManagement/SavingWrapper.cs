using System.Collections;
using Saving;
using UnityEngine;

namespace SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private KeyCode saveKey = KeyCode.S;
        [SerializeField] private KeyCode loadKey = KeyCode.L;
        [SerializeField] private KeyCode deleteKey = KeyCode.Delete;
        [SerializeField] private float fadeInTime = 1f;
        
        private const string DefaultSaveFile = "save";
        
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(DefaultSaveFile);
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(saveKey))
            {
                Save();
            }

            if (Input.GetKeyDown(loadKey))
            {
                Load();
            }
            
            if (Input.GetKeyDown(deleteKey))
            {
                Delete();
            }
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }

        private void Delete()
        {
            _savingSystem.Delete(DefaultSaveFile);
        }
    }
}
