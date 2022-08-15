using Core;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject equippedPrefab;
        [SerializeField] private float weaponRange;
        [SerializeField] private float weaponDamage;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;
        
        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            var handTransform = GetHandTransform(rightHand, leftHand);
            
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public bool HasProjectile() => projectile != null;

        public void LunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(
                projectile, GetHandTransform(rightHand, leftHand).position,
                Quaternion.identity);
            
            projectileInstance.SetTarget(target, weaponDamage);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand) => 
            isRightHanded ? rightHand : leftHand;
    }
}
