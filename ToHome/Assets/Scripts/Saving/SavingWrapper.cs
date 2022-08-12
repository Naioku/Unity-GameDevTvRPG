using UnityEngine;

namespace Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private KeyCode saveKey = KeyCode.S;
        [SerializeField] private KeyCode loadKey = KeyCode.L;
        
        private const string DefaultSaveFile = "save";

        private void Update()
        {
            var savingSystem = GetComponent<SavingSystem>();
            if (Input.GetKeyDown(saveKey))
            {
                savingSystem.Save(DefaultSaveFile);
            }

            if (Input.GetKeyDown(loadKey))
            {
                savingSystem.Load(DefaultSaveFile);
            }
        }
    }
}
