using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class ReverseFillable : Fillable
    {
        public Image ReverseImage;

        public override void Activate(float Duration)
        {
            base.Activate(Duration);
            LeanTween.rotateZ(ReverseImage.gameObject, 180, 1f).setLoopPingPong();
        }

        public override void Deactivate()
        {
            LeanTween.cancel(ReverseImage.gameObject);
            base.Deactivate();
        }

        public override void EndGame()
        {
            base.EndGame();
            LeanTween.cancel(ReverseImage.gameObject);
        }
    }
}