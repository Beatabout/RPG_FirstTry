using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
              
        }
        void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControls;
            GetComponent<PlayableDirector>().stopped += EnanleControls;
        }

        void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControls;
            GetComponent<PlayableDirector>().stopped -= EnanleControls;
        }

        void DisableControls(PlayableDirector pb){
            print("DisableControls");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnanleControls(PlayableDirector pb){
            print("EnanleControls");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
