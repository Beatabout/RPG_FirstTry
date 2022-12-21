using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        [SerializeField] TMP_Text levelText;

        void Start() {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        void Update(){
            levelText.text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}