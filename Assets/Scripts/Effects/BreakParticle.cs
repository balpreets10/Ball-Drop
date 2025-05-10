using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakParticle : MonoBehaviour
{
    public ParticleSystem breakParticles;
    ParticleSystem.MainModule main;

    private void Awake()
    {
        breakParticles = GetComponent<ParticleSystem>();
        main = breakParticles.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void Activate(Vector3 position,Color color)
    {
        gameObject.SetActive(true);
        transform.localPosition = position;
        main.startColor = color;
        breakParticles.Play();
    }

    public void OnParticleSystemStopped()
    {
        breakParticles.Stop();
        gameObject.SetActive(false);
    }

    public bool activeInHierarchy { get { return gameObject.activeInHierarchy; } }
}
