using TMPro;
using UnityEngine;

namespace Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText("{0:0}", _experience.GetExperiencePoints());
        }
    }
}
