using System;
using System.Collections;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = -1f;

        bool isDead = false;
        
        void Start() {
            if(health < 0) 
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead(){
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage){
            health = Mathf.Max(health - damage, 0);
            print(gameObject.name + "'s health is: " + health);
            if(health <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if(experience == null) return;
            float reward = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
            experience.GainExprerience(reward);
        }

        public float GetPercentage(){
            float maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            return (health / maxHealth) * 100;
        }

        private void Die()
        {
            if(isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }
        
        public void RestoreState(object state)
        {
            health = (float)state;
            if(health <= 0){
                Die();
            }
        }

    } 
}


