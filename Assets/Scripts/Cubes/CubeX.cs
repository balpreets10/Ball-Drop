using BallDrop.Audio;
using BallDrop.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeX : BaseCube
    {
        public GameObject X;

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            //Debug.Log("Activating X cube");
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            LeanTween.rotateAround(X, Vector3.up, 360, 1f).setLoopPingPong();
        }

        public override void Deactivate()
        {           
            LeanTween.cancel(X);
            X.transform.localRotation = Quaternion.identity;
            base.Deactivate();
        }

        private void GetRenderer()
        {
            m_Renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        }
    }
}