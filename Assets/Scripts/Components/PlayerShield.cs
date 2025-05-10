using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class PlayerShield : MonoBehaviour
    {
        public GameObject Sphere;
        public ParticleSystem Beams;
        private ParticleSystem.MainModule beamMainModule;
        public Renderer ShieldRenderer;
        private Material ShieldMaterial;

        private void Awake()
        {
            ResetScale();
            beamMainModule = Beams.main;
            ShieldMaterial = ShieldRenderer.material;
        }
        public void Activate(Color color)
        {
            ResetScale();
            beamMainModule.startColor = ColorData.Instance.GetSecondaryColor();
            Beams.Play();
            Sphere.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            LeanTween.scale(gameObject, Vector3.one, .5f).setEase(LeanTweenType.easeOutBack);
            LeanTween.value(gameObject, 0.2f, -0.8f, 1f).setLoopPingPong().setOnUpdate(UpdateFill);

        }

        private void UpdateFill(float power)
        {
            ShieldMaterial.SetFloat("_Fill", power);
        }

        public void Deactivate()
        {
            Beams.Stop();
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, Vector3.zero, .5f).setEase(LeanTweenType.easeInBack);
            LeanTween.value(gameObject, ShieldMaterial.GetFloat("_Fill"), -0.8f, .5f);
        }

        //private void StopTweens()
        //{
        //    LeanTween.cancel(gameObject);
        //}

        private void ResetScale()
        {

            transform.localScale = Vector3.zero;

        }
    }

}