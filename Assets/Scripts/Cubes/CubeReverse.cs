using BallDrop.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeReverse : BaseCube
    {
        public GameObject LeftArrow;
        public GameObject RightArrow;

        private Vector3 LeftInitialPosition, RightInitialPosition;

        protected override void Awake()
        {
            base.Awake();
            LeftInitialPosition = LeftArrow.transform.localPosition;
            RightInitialPosition = RightArrow.transform.localPosition;
        }

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            LeftInitialPosition = LeftArrow.transform.localPosition;
            RightInitialPosition = RightArrow.transform.localPosition;
            AnimateArrows();
        }

        private void AnimateArrows()
        {
            if (LeftArrow != null)
                LeanTween.moveLocalZ(LeftArrow, 0.01f, .5f).setLoopPingPong();
            if (RightArrow != null)
                LeanTween.moveLocalZ(RightArrow, -0.01f, .5f).setLoopPingPong();
        }

        public override void Deactivate()
        {
            if (LeftArrow != null)
                LeanTween.cancel(LeftArrow);
            if (RightArrow != null)
                LeanTween.cancel(RightArrow);
            base.Deactivate();
        }
    }
}