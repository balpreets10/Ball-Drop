using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop.Interfaces
{
    public interface IPowerup
    {
        string name { get; set; }

        bool activeInHierarchy { get; }

        void ActivateAndSetPosition(Vector3 pos);

        PowerupType GetPowerupType();

        float GetPowerupDuration();

        void PlayPowerupSound();

        void SetActive(bool active);

        void Deactivate();
  
    }
}