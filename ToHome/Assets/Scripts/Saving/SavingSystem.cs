using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                RestoreState(binaryFormatter.Deserialize(stream));
            }
        }

        private string GetPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }

        private object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var entity in FindObjectsOfType<SavableEntity>())
            {
                state[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            return state;
        }

        private void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>) state;
            foreach (var entity in FindObjectsOfType<SavableEntity>())
            {
                entity.RestoreState(stateDict[entity.GetUniqueIdentifier()]);
            }
        }
    }
}
