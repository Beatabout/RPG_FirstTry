using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")){
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition(){
            if(sceneToLoad < 0){
                print($"Scene to load not on {this.name}");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeIn(fadeInTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSecondsRealtime(fadeWaitTime);
            yield return fader.FadeOut(fadeOutTime);
            Destroy(gameObject);
            
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position); 
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != this.destination) continue;
                    return portal;
            }
            return null;
        }
    }

}

