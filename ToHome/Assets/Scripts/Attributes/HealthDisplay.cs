using TMPro;
using UnityEngine;

namespace Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText("{0:0}%", _health.GetPercentage());
        }
    }
}
