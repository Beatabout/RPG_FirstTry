using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 2f;
        Health target;
        Animator animator;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.IsDead()) return;
            
            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange; 
                
            if(!isInRange){
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            } else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }

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
            target.TakeDamage(weaponDamage);
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
    }
}


