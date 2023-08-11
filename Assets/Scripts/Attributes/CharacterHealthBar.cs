using System;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class CharacterHealthBar : MonoBehaviour
    {
        [SerializeField] Slider healthBar;
        [SerializeField] TMP_Text levelText;

        BaseStats baseStats;

        void Start() {
            baseStats = GetComponentInParent<BaseStats>();
            UpdateHealthBar();
        }

        public void UpdateHealthBar(){
            healthBar.value = GetComponentInParent<Health>().GetPercentage() / 100;
            if(baseStats.gameObject.CompareTag("Player"))
            {
                levelText.text = String.Format("{0:0}", baseStats.GetLevel());
            } 
            else
            {
                levelText.text = String.Format("{0:0}", baseStats.GetStartingLevel());
            }
        }
    }
}