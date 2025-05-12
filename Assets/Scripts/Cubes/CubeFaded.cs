using BallDrop.Audio;
using BallDrop.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeFaded : BaseCube
    {
        private Material material;
        private Vector3 initialScale = new Vector3(1, 0.3f, 1);

        protected override void Start()
        {
            base.Start();
            material = m_Renderer.sharedMaterial;
        }

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            transform.localScale = initialScale;
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            LeanTween.value(1f, .3f, .8f).setLoopPingPong().setOnUpdate(UpdateMaterialAlpha);
        }

        private void UpdateMaterialAlpha(float alpha)
        {
            material.SetFloat("_CubeAlpha", alpha);
        }

        public override void Deactivate()
        {
            //LeanTween.cancel(gameObject);
            base.Deactivate();
        }

        public override void PlayBounceSound()
        {
            AudioManager.Instance.PlayEffect(m_BounceSoundNormal);
        }

        public void Disappear()
        {
            transform.localScale = Vector3.zero;
        }
    }
}