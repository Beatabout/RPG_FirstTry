using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon = null;

        void Awake() {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start() {

            currentWeapon.ForceInit();
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.IsDead()) return;
            
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.WeaponRange; 
                
            if(!isInRange){
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            } else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget(){
            return target;
        }

        public bool CanAttack(GameObject combatTarget){
            if(combatTarget != null && !combatTarget.GetComponent<Health>().IsDead()){
                return true;
            } else {
                return false;
            }
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack < timeBetweenAttacks) return;
            //This will trigger the Hit() event
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
            timeSinceLastAttack = 0;
        }

        //Animation Event
        void Hit(){
            if(target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value.HasProjectule()){
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } else {
                
                target.TakeDamage(gameObject, damage);
            }
        }
        //Animation Event
        void Shoot(){
           Hit();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            print(currentWeapon.value.name);
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

    }
}


