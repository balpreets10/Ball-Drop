using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class CircleSplatterEffect : BaseSplatter
    {
        public ParticleSystem mParticleSystem;

        public override void ActivateAndSetParent(Vector3 position, Transform parent)
        {
            base.ActivateAndSetParent(position, parent);
            if (!mParticleSystem.isPlaying)
                mParticleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            Deactivate();
        }
    }
}