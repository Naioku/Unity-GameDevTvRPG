using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int) state["lastSceneBuildIndex"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            
            RestoreState(state);
        }
        
        public void Save(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            CaptureState(state);
            SaveFile(fileName, state);
            print("Game saved.");
        }

        public void Load(string fileName)
        {
            RestoreState(LoadFile(fileName));
            print("Game loaded.");
        }

        public void Delete(string fileName)
        {
            File.Delete(GetPathFromSaveFile(fileName));
            print("Game save deleted.");
        }

        private void SaveFile(string fileName, object state)
        {
            string path = GetPathFromSaveFile(fileName);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (Dictionary<string, object>) binaryFormatter.Deserialize(stream);
            }
        }

        private string GetPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var entity in FindObjectsOfType<SavableEntity>())
            {
                state[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var entity in FindObjectsOfType<SavableEntity>())
            {
                string id = entity.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    entity.RestoreState(state[id]);
                }
            }
        }
    }
}
