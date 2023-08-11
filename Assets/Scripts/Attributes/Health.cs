using System;
using System.Collections;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        float health = -1f;
        bool isDead = false;

        BaseStats baseStats;

        void Awake() {
            baseStats = GetComponent<BaseStats>();
        }
        
        void Start() {
            if(health < 0)
            {
                health = GetMaxHealthPoints();
            }
        }

        void OnEnable() {
            baseStats.onLevelUp += RegenerateHealth;
            baseStats.onLevelUp += UpdateHealthBar;
        }

        void OnDisable() {
            baseStats.onLevelUp -= RegenerateHealth;
            baseStats.onLevelUp -= UpdateHealthBar;
        }

        void RegenerateHealth(){
            float renegHealthPoints = baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100);
            health = Mathf.Max(health, renegHealthPoints);
        }

        void UpdateHealthBar() {
            GetComponent<CharacterHealthBar>().UpdateHealthBar();
        }

        public bool IsDead(){
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage){

            health = Mathf.Max(health - damage, 0);
            GetComponent<CharacterHealthBar>().UpdateHealthBar();
            if(health <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
            takeDamage.Invoke(damage);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if(experience == null) return;
            float reward = baseStats.GetStat(Stat.ExperienceReward);
            experience.GainExprerience(reward);
        }

        public float GetHealthPoints()
        {
            return health;
        }

        public float GetMaxHealthPoints(){
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage(){
            if(health < 0 && !isDead){
                health = GetMaxHealthPoints();
            }
            float maxHealth = baseStats.GetStat(Stat.Health);
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


