using BallDrop.Interfaces;
using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class SettingsScrollItem : ScrollItem
    {
        public Slider PlayerMovementSentivitySlider, MusicVolSlider, EffectsVolSlider;

        private void Start()
        {
            PlayerMovementSentivitySlider.value = PreferenceManager.Instance.GetIntPref(PrefKey.MovementSensitivity, 8);
            MusicVolSlider.value = PreferenceManager.Instance.GetFloatPref(PrefKey.Music, 0.05f);
            EffectsVolSlider.value = PreferenceManager.Instance.GetFloatPref(PrefKey.Effect, 0.1f);
        }
        
        public void ChangeSensitivity(float value)
        {
            PreferenceManager.Instance.UpdateIntPref(PrefKey.MovementSensitivity, (int)value);
        }

        public void ChangeMusicVolume(float value)
        {
            MyEventManager.Instance.OnMusicVolumeChanged.Dispatch(value);
        }

        public void ChangeEffectVolume(float value)
        {
            MyEventManager.Instance.OnEffectVolumeChanged.Dispatch(value);
        }

        public void ChangeBackground()
        {
            MyEventManager.Instance.ChangeTexture.Dispatch();
        }
    }
}