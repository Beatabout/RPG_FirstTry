using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        [SerializeField] TMP_Text healthText;

        void Awake() {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        void Update() {
            healthText.text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}
