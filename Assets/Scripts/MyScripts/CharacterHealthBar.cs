using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    void Awake() {
        transform.rotation = Camera.main.transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        healthBar.value = GetComponentInParent<Health>().GetPercentage() / 100;
    }
}
