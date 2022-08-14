using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        private static readonly Dictionary<string, SavableEntity> GlobalLookup = new();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            foreach (var savable in GetComponents<ISavable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }

            return state;
        }
        
        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>) state;
            foreach (var savable in GetComponents<ISavable>())
            {
                string typeString = savable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    savable.RestoreState(stateDict[typeString]);
                }
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;
            if (GlobalLookup[candidate] == this) return true;
            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }
            return false;
        }
    }
}
