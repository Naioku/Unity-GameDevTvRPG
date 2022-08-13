using System;
using System.Collections;
using UnityEngine;

namespace Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private KeyCode saveKey = KeyCode.S;
        [SerializeField] private KeyCode loadKey = KeyCode.L;
        
        private const string DefaultSaveFile = "save";
        
        private SavingSystem _savingSystem;

        private IEnumerator Start()
        {
            _savingSystem = GetComponent<SavingSystem>();
            yield return _savingSystem.LoadLastScene(DefaultSaveFile);
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
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }
    }
}
