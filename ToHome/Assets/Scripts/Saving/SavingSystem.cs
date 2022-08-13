using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;

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
                Transform playerTransform = GetPlayerTransform();
                var binaryFormatter = new BinaryFormatter();
                var position = new SerializableVector3(playerTransform.position);
                binaryFormatter.Serialize(stream, position);
            }
        }

        public void Load(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                var position = (SerializableVector3) binaryFormatter.Deserialize(stream);
                
                var navMeshAgent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = false;
                
                Transform playerTransform = GetPlayerTransform();
                playerTransform.position = position.ToVector();
                
                navMeshAgent.enabled = true;

            }
        }
        
        private string GetPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }
        
        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }
    }
}
