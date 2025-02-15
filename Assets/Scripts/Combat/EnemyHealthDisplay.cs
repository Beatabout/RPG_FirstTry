using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        [SerializeField] TMP_Text healthText;

        void Awake() {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        void Update() {
            if(fighter.GetTarget() != null) {
                Health health = fighter.GetTarget();
                healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
            else
            {
                healthText.text = "N/A";
            }
        }
    }
}
