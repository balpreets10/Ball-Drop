using BallDrop.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeEnemy : BaseCube
    {
        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            SetColor(ColorData.Instance.GetSecondaryColor());
            base.ActivateAndSetPosition(parent, zpos);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}