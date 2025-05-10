using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class Billboard : MonoBehaviour
    {
        private Transform CameraTransform;

        public void SetCamera()
        {
            CameraTransform = GameData.Instance.cameraController.MainCamera.transform;
            transform.forward = CameraTransform.forward;

        }
    }
}