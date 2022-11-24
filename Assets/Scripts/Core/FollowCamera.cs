using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float rotationSpeed = 25f;
        float cameraRotationY;
        float cameraRotationX;
        float cameraRoatatitonY;
        float cameraRoatatitonX;

        void Start() {
            transform.rotation = target.rotation;
            cameraRoatatitonY = target.rotation.eulerAngles.y;
            cameraRoatatitonX = target.rotation.eulerAngles.x;
        }

        void LateUpdate()
        {
            
            cameraRotationY = Input.GetAxis("Horizontal");
            cameraRotationX = Input.GetAxis("Vertical");
            #if UNITY_ANDROID
                cameraRoatatitonY = target.transform.rotation.eulerAngles.y;
            #elif UNINTY_EDITOR
                cameraRoatatitonY += cameraRotationY * rotationSpeed * Time.deltaTime;
            #endif
            cameraRoatatitonY += cameraRotationY * rotationSpeed * Time.deltaTime;
            cameraRoatatitonX += cameraRotationX * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(cameraRoatatitonX ,cameraRoatatitonY ,0);

            transform.position = target.position;
        }
    }
}



