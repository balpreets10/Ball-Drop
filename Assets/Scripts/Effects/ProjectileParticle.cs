using BallDrop.Base;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class ProjectileParticle : MonoBehaviour
    {
        public float timeMultiplier;
        float time;
        bool activated;
        public List<ParticleSystem> particleSystems;

        public bool activeInHierarchy { get { return gameObject.activeInHierarchy; } }

        public void Activate(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.localPosition = position;
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
            time = GameData.Instance.BallBounceDistance / GameData.Instance.BallBounceSpeed;
            activated = true;

        }

        private void Update()
        {
            if (activated)
                transform.Translate(Vector3.down * Time.deltaTime * time * timeMultiplier);
        }

        public void Deactivate()
        {
            activated = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            BaseCube cube = null;
            try
            {
                cube = other.GetComponent<BaseCube>();
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Not a Cube");
            };
            if (cube != null)
            {
                Deactivate();
            }
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}