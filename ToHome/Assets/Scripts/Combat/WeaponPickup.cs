using UnityEngine;

namespace Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weaponSO;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.tag.Equals("Player")) return;
            
            other.GetComponent<Fighter>().EquipWeapon(weaponSO);
            Destroy(gameObject);
        }
    }
}
