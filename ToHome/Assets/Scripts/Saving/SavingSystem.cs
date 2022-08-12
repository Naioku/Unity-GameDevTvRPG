using System;
using System.IO;
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
                byte[] buffer = SerializeVector(playerTransform.position);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Load(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                var navMeshAgent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = false;
                
                Transform playerTransform = GetPlayerTransform();
                playerTransform.position = DeserializeVector(buffer);
                
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

        private byte[] SerializeVector(Vector3 vector)
        {
            var vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            
            return vectorBytes;
        }
        
        private Vector3 DeserializeVector(byte[] buffer)
        {
            var vector = new Vector3();
            vector.x = BitConverter.ToSingle(buffer, 0);
            vector.y = BitConverter.ToSingle(buffer, 4);
            vector.z = BitConverter.ToSingle(buffer, 8);

            return vector;
        }
    }
}
