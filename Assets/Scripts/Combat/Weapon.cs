using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {

        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        public float WeaponRange { get { return weaponRange; }}
        public float WeaponDamage { get { return weaponDamage; }}

        public void Spawn(Transform handTransform, Animator animator){
            if(equippedPrefab != null){
                Instantiate(equippedPrefab, handTransform);
            }
            if(weaponOverride != null){
                animator.runtimeAnimatorController = weaponOverride;
            }
        }
    }
}