using Attributes;
using TMPro;
using UnityEngine;

namespace Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health health = _fighter.GetTarget();

            if (health == null)
            {
                GetComponent<TextMeshProUGUI>().SetText("N/A");
                return;
            }
            
            GetComponent<TextMeshProUGUI>().SetText("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
