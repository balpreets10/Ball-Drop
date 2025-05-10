using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class SplatterEffect : BaseSplatter
    {
        public MeshRenderer meshRenderer;
        Vector3 position = Vector3.zero;

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public override void Activate(Vector3 position)
        {
            base.Activate(position);
            LeanTween.moveLocalX(gameObject, .85f, 0f);
            meshRenderer.material.SetColor("_SplatColor", ColorData.Instance.GetSecondaryColor());
            meshRenderer.material.SetColor("_DissolveColor", ColorData.Instance.GetPrimaryColor());
            LeanTween.value(0.8f, 0.2f, .25f).setOnUpdate(UpdateDissolve).setLoopPingPong(1).setOnComplete(Deactivate);
        }

        private void UpdateDissolve(float value)
        {
            meshRenderer.material.SetFloat("_DissolveAmount", value);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            meshRenderer.material.SetFloat("_DissolveAmount", 0);
        }


    }
}