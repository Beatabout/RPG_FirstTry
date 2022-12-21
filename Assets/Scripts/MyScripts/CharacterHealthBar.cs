using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text levelText;

    BaseStats baseStats;
    void Awake() {
        transform.rotation = Camera.main.transform.rotation;
        baseStats = GetComponentInParent<BaseStats>();
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        healthBar.value = GetComponentInParent<Health>().GetPercentage() / 100;
        if(baseStats.gameObject.CompareTag("Player"))
        {
            levelText.text = String.Format("{0:0}", baseStats.GetLevel());
        } 
        else
        {
            levelText.text = String.Format("{0:0}", baseStats.GetStartingLEvel());
        }
    }
}
