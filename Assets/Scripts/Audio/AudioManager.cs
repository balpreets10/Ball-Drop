using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        // Audio players components.
        public List<AudioSource> EffectsSource;

        [Tooltip("The Last Audio Source in the added sources is the music source")]
        public AudioSource MusicSource;

        public AudioClip[] BackgroundMusic;

        private float MusicSourceMaxVolume;

        // Random pitch adjustment range.
        public float LowPitchRange = .95f;

        public float HighPitchRange = 1.05f;

        public enum MusicState
        {
            Play,
            Pause,
            UnPause,
            Stop
        }

        private void OnEnable()
        {
            MyEventManager.Menu.OnMusicVolumeChanged.AddListener(OnMusicVolumeChanged);
            MyEventManager.Menu.OnEffectVolumeChanged.AddListener(OnEffectVolumeChanged);
            MyEventManager.Game.PauseGame.AddListener(OnGamePause);
            MyEventManager.Game.EndGame.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            MyEventManager.Menu.OnMusicVolumeChanged.RemoveListener(OnMusicVolumeChanged);
            MyEventManager.Menu.OnEffectVolumeChanged.RemoveListener(OnEffectVolumeChanged);
            MyEventManager.Game.PauseGame.RemoveListener(OnGamePause);
            MyEventManager.Game.EndGame.RemoveListener(OnGameEnd);
        }

        private void Start()
        {
            foreach (AudioSource source in EffectsSource)
            {
                if (source != null)
                {
                    source.loop = false;
                    source.volume = PreferenceManager.Instance.GetFloatPref(PrefKey.Effect, 0.1f);
                    source.Stop();
                    source.playOnAwake = false;
                    source.clip = null;
                    source.mute = false;
                }
            }
            if (MusicSource != null)
                MusicSourceMaxVolume = MusicSource.volume = PreferenceManager.Instance.GetFloatPref(PrefKey.Music, 0.05f);
        }

        public AudioSource GetEffectSource()
        {
            foreach (AudioSource source in EffectsSource)
            {
                if (source != null)
                {
                    if (source.clip == null || !source.isPlaying)
                        return source;
                }
            }
            return null;
        }

        // Play a single clip through the sound effects source.
        public void PlayEffect(AudioClip clip)
        {
            AudioSource source = GetEffectSource();
            if (source != null)
            {
                source.clip = clip;
                LeanTween.value(0, 1, clip.length).setOnComplete(RemoveClipFromSource, source);
                source.Play();
            }
            else
            {
                Debug.Log("No Audio Source Found");
            }
        }

        private void RemoveClipFromSource(object param)
        {
            AudioSource source = (AudioSource)param;
            if (source != null)
            {
                source.Stop();
                source.clip = null;
            }
        }

        // Play a single clip through the music source.
        public void PlayMusic(AudioClip clip)
        {
            if (MusicSource != null)
            {
                MusicSource.clip = clip;
            }
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            MusicSourceMaxVolume = MusicSource.volume = volume;
            PreferenceManager.Instance.UpdateFloatPref(PrefKey.Music, volume);
        }

        private void OnEffectVolumeChanged(float volume)
        {
            foreach (AudioSource source in EffectsSource)
            {
                if (source != null)
                    source.volume = volume;
            }
            PreferenceManager.Instance.UpdateFloatPref(PrefKey.Effect, volume);
        }

        private void OnGameEnd()
        {
            LeanTween.value(gameObject, OnVolumeChanged, MusicSource.volume, 0, .2f);
        }

        private void OnGamePause()
        {
            if (MusicSource != null)
                MusicSource.Pause();
        }

        private void OnGameStart()
        {
            PlayBgMusic();
        }

        public void Unpause()
        {
            if (MusicSource != null)
                MusicSource.UnPause();
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }

        private void OnVolumeChanged(float volume)
        {
            if (MusicSource != null)
                MusicSource.volume = volume;
        }

        public void PlayBgMusic()
        {
            int randomIndex = UnityEngine.Random.Range(0, BackgroundMusic.Length);
            if (MusicSource != null)
            {
                MusicSource.clip = BackgroundMusic[randomIndex];
                MusicSource.Stop();
                MusicSource.Play();
            }
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }
    }
}