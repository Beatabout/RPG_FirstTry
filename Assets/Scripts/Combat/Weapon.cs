using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {

        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        GameObject weaponInstance;

        public float WeaponRange { get { return weaponRange; }}
        public float WeaponDamage { get { return weaponDamage; }}

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator){
            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weaponInstance = Instantiate(equippedPrefab, handTransform);
            }
            // повертає стандартний контроллер при відсутності оверрайд контроллера на зброї
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponOverride != null){
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if(overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public bool HasProjectule()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public void DestroySelf(){
            Destroy(weaponInstance);
        }
    }
}