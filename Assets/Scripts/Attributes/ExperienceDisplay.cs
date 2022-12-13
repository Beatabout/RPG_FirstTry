using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        [SerializeField] TMP_Text exprerienceText;

        void Start() {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        void Update(){
            exprerienceText.text = String.Format("{0:0}", experience.GetPoints());
        }
    }
}