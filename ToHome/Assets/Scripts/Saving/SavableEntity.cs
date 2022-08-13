using System;
using Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }
        
        public void RestoreState(object state)
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            var navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3) state).ToVector();
            navMeshAgent.enabled = true;
        }

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
