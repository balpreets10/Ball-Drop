using BallDrop.Audio;
using BallDrop.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeNormal : BaseCube
    {
        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            //Debug.Log("Activating normal");
            SetColor(ColorData.Instance.GetPrimaryColor());
            base.ActivateAndSetPosition(parent, zpos);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}