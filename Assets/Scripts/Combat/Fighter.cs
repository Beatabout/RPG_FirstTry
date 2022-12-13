using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Start() {

            if(currentWeapon == null){
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.IsDead()) return;
            
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange; 
                
            if(!isInRange){
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            } else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon){
            currentWeapon = weapon;
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
             if(currentWeapon.HasProjectule()){
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            } else {
                target.TakeDamage(gameObject, currentWeapon.WeaponDamage);
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}


