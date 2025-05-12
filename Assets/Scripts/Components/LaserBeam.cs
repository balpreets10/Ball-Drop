using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class LaserBeam : GameComponent
    {
        public LineRenderer lineRenderer;
        public BoxCollider boxCollider;

        private Vector3 colliderSize;
        private Vector3 colliderCenter = Vector3.zero;
        private Vector3 initialPosition;
        private float YDistance = -15.0f;

        protected override void Awake()
        {
            base.Awake();
            colliderSize = boxCollider.size;
        }

        public void Activate(Vector3 InitialPosition, float duration)
        {
            Activate();
            initialPosition = InitialPosition;
            if (lineRenderer != null)
                lineRenderer.SetPosition(0, InitialPosition);
            colliderCenter.z = InitialPosition.z;
            colliderCenter.y = InitialPosition.y;
            LeanTween.value(gameObject, UpdateLineRenderer, InitialPosition, InitialPosition + new Vector3(0f, YDistance, 0f), duration).setOnComplete(Deactivate);
            LeanTween.value(0, YDistance, duration).setOnUpdate(UpdateCollider);
        }

        private void UpdateLineRenderer(Vector3 newPosition)
        {
            if (lineRenderer != null)
                lineRenderer.SetPosition(1, newPosition);
        }

        private void UpdateCollider(float value)
        {
            colliderSize.y = value;
            colliderCenter.y = initialPosition.y + (value / 2.0f);
            if (boxCollider != null)
            {
                boxCollider.size = colliderSize;
                boxCollider.center = colliderCenter;
            }
        }

        public void SetName(string value)
        {
            gameObject.name = value;
        }
    }
}