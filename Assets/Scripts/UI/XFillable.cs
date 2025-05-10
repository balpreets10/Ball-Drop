using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class XFillable : Fillable
    {
        public Outline ScaleImageOutline;
        LTDescr tween;
        Vector2 effectDistance = Vector2.zero;

        public override void Activate(float Duration)
        {
            ScaleImageOutline.gameObject.transform.localScale = Vector3.one;
            base.Activate(Duration);
            tween = LeanTween.value(0, 4, .5f).setLoopPingPong().setOnUpdate(UpdateGlow);
            LeanTween.scale(ScaleImageOutline.gameObject, Vector3.one * 0.8f, 2.5f).setLoopType(LeanTweenType.pingPong);
        }

        private void UpdateGlow(float value)
        {
            effectDistance.x = value;
            effectDistance.y = value;
            if (ScaleImageOutline != null)
                ScaleImageOutline.effectDistance = effectDistance;
        }

        public override void Deactivate()
        {
            LeanTween.cancel(ScaleImageOutline.gameObject);
            base.Deactivate();
        }

        public override void EndGame()
        {
            LeanTween.cancel(tween.id);
            LeanTween.cancel(ScaleImageOutline.gameObject);
            base.EndGame();
        }
    }
}