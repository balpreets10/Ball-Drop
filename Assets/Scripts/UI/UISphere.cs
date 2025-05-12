using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class UISphere : UIComponent
    {
        public float XDistance;
        public float YDistance;
        private TrailRenderer lineRenderer;
        private RectTransform rectTransform;

        protected override void Awake()
        {
            base.Awake();
            lineRenderer = GetComponent<TrailRenderer>();
            rectTransform = GetComponent<RectTransform>();
        }

        protected override void Start()
        {
            base.Start();
            StartTween();
        }

        private void StartTween()
        {
            LeanTween.rotateAround(gameObject, Vector3.one, 360, .5f).setLoopType(LeanTweenType.linear);
            LeanTween.moveLocalY(gameObject, -Screen.height, 2.5f).setOnComplete(ResetSphere).setDelay(1f);
            lineRenderer.enabled = true;
        }

        private void ResetSphere()
        {
            lineRenderer.enabled = false;
            rectTransform.anchoredPosition3D = new Vector3(UnityEngine.Random.Range(-500, 500), 200, 0);
            StartTween();
        }
    }
}