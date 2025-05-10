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
            MyEventManager.OnMusicVolumeChanged.AddListener(OnMusicVolumeChanged);
            MyEventManager.OnEffectVolumeChanged.AddListener(OnEffectVolumeChanged);
            MyEventManager.PauseGame.AddListener(OnGamePause);
            MyEventManager.EndGame.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
                MyEventManager.OnMusicVolumeChanged.RemoveListener(OnMusicVolumeChanged);
                MyEventManager.OnEffectVolumeChanged.RemoveListener(OnEffectVolumeChanged);
                MyEventManager.PauseGame.RemoveListener(OnGamePause);
                MyEventManager.EndGame.RemoveListener(OnGameEnd);
        }

        private void Start()
        {
            foreach (AudioSource source in EffectsSource)
            {
                source.loop = false;
                source.volume = PreferenceManager.Instance.GetFloatPref(PrefKey.Effect, 0.1f);
                source.Stop();
                source.playOnAwake = false;
                source.clip = null;
                source.mute = false;
            }
            MusicSourceMaxVolume = MusicSource.volume = PreferenceManager.Instance.GetFloatPref(PrefKey.Music, 0.05f);
        }

        public AudioSource GetEffectSource()
        {
            foreach (AudioSource source in EffectsSource)
            {
                if (source.clip == null || !source.isPlaying)
                    return source;
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
            source.Stop();
            source.clip = null;
        }

        // Play a single clip through the music source.
        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            //musicState = MusicState.Play;
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }

        // Play a random clip from an array, and randomize the pitch slightly.
        //public void RandomSoundEffect(params AudioClip[] clips)
        //{
        //    int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        //    //float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);

        //    //EffectsSource.pitch = randomPitch;
        //    EffectsSource.clip = clips[randomIndex];
        //    EffectsSource.Play();
        //}

        private void OnMusicVolumeChanged(float volume)
        {
            MusicSourceMaxVolume = MusicSource.volume = volume;
            PreferenceManager.Instance.UpdateFloatPref(PrefKey.Music, volume);
        }

        private void OnEffectVolumeChanged(float volume)
        {
            foreach (AudioSource source in EffectsSource)
                source.volume = volume;
            PreferenceManager.Instance.UpdateFloatPref(PrefKey.Effect, volume);
        }



        private void OnGameEnd()
        {
            LeanTween.value(gameObject, OnVolumeChanged, MusicSource.volume, 0, .2f);
        }

        private void OnGamePause()
        {
            MusicSource.Pause();
        }

        private void OnGameStart()
        {
            PlayBgMusic();
        }

        public void Unpause()
        {
            MusicSource.UnPause();
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }

        private void OnVolumeChanged(float volume)
        {
            MusicSource.volume = volume;
        }

        public void PlayBgMusic()
        {
            int randomIndex = UnityEngine.Random.Range(0, BackgroundMusic.Length);
            MusicSource.clip = BackgroundMusic[randomIndex];
            MusicSource.Stop();
            MusicSource.Play();
            LeanTween.value(gameObject, OnVolumeChanged, 0, MusicSourceMaxVolume, .2f);
        }
    }
}