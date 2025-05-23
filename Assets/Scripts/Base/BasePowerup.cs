﻿using BallDrop.Audio;
using BallDrop.Interfaces;
using UnityEngine;

namespace BallDrop.Base
{
    public class BasePowerup : GameComponent, IPowerup
    {
        [SerializeField]
        private PowerupType powerType;

        [SerializeField]
        private float Duration;

        [SerializeField]
        private AudioClip PowerUpSound;

        bool IPowerup.activeInHierarchy { get { return gameObject.activeInHierarchy; } }

        string IPowerup.name { get { return gameObject.name; } set { gameObject.name = value; } }

        private void OnEnable()
        {
            MyEventManager.Game.EndGame.AddListener(EndGame);
        }

        private void OnDisable()
        {
            MyEventManager.Game.EndGame.RemoveListener(EndGame);
        }

        private void EndGame()
        {
            Deactivate();
        }

        public void ActivateAndSetPosition(Vector3 pos)
        {
            Activate();
            gameObject.transform.SetPositionAndRotation(pos, Quaternion.identity);
            if (powerType == PowerupType.Shield)
            {
                LeanTween.rotateAround(gameObject, Vector3.up, 360, 1f).setEase(GameData.Instance.GetRandomTweenType()).setLoopType(LeanTweenType.linear);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GameStrings.TopBoundary))
            {
                Deactivate();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            gameObject.transform.SetParent(ObjectPool.Instance.PooledObjectsHolder);
        }

        public PowerupType GetPowerupType()
        {
            return powerType;
        }

        public float GetPowerupDuration()
        {
            return Duration;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        public IPowerup GetIPowerup()
        {
            return this;
        }

        public void PlayPowerupSound()
        {
            if (PowerUpSound != null)
                AudioManager.Instance.PlayEffect(PowerUpSound);
        }
    }
}