using UnityEngine;

namespace Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string fileName)
        {
            print("Saving to " + fileName);
        }
        
        public void Load(string fileName)
        {
            print("Loading from " + fileName);
        }
    }
}
