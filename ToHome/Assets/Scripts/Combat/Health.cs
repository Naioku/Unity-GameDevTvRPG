using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Animator))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        public bool IsDead { get; private set; }
        private static readonly int Die = Animator.StringToHash("die");

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (!IsDead && health == 0f)
            {
                GetComponent<Animator>().SetTrigger(Die);
                IsDead = true;
            }
        }
    }
}
