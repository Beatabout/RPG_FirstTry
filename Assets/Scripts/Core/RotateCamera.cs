using UnityEngine;

namespace RPG.Core
{
    public class RotateCamera : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 25f;
        float inputRotationY;
        float inputRotationX;
        float cameraRotatitonY;
        float cameraRotatitonX;

        void Awake() {
            transform.rotation = transform.rotation;
            cameraRotatitonY = transform.rotation.eulerAngles.y;
            cameraRotatitonX = transform.rotation.eulerAngles.x;
        }

        void LateUpdate()
        {
            
            inputRotationY = Input.GetAxis("Horizontal");
            inputRotationX = Input.GetAxis("Vertical");
            cameraRotatitonY += inputRotationY * rotationSpeed * Time.deltaTime * 10;
            cameraRotatitonX += inputRotationX * rotationSpeed * Time.deltaTime * 10;
            transform.rotation = Quaternion.Euler(cameraRotatitonX ,cameraRotatitonY, 0);
        }
    }
}



