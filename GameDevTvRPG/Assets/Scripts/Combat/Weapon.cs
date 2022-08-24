using Attributes;
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
        [SerializeField] private float percentageBonus;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;

        private const string WeaponName = "Weapon";
        
        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;
        public float PercentageBonus => percentageBonus;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            
            if (equippedPrefab != null)
            {
                var handTransform = GetHandTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = WeaponName;

            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WeaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile() => projectile != null;

        public void LunchProjectile(GameObject instigator, Transform rightHand, Transform leftHand, Health target, float damage)
        {
            Projectile projectileInstance = Instantiate(
                projectile, 
                GetHandTransform(rightHand, leftHand).position,
                Quaternion.identity);
            
            projectileInstance.SetTarget(instigator, target, damage);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand) => 
            isRightHanded ? rightHand : leftHand;
    }
}
