using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool isPlayed = false;

        private void OnTriggerEnter(Collider other) {
            if(!other.CompareTag("Player") || isPlayed) return;
            isPlayed = true;
            GetComponent<PlayableDirector>().Play();
        }
    }
}