using UnityEngine;

namespace Core
{
    [RequireComponent(
        typeof(Animator),
        typeof(ActionScheduler))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        public bool IsDead { get; private set; }
        private static readonly int DieAnimationName = Animator.StringToHash("die");

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;
            
            GetComponent<Animator>().SetTrigger(DieAnimationName);
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
