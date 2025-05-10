using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class Sound : MonoBehaviour
    {
        //public string SoundName;
        //public AudioClip clip;
        //public AudioSource source;
        //public Action<Sound> callback;
        //public bool loop;
        //public bool interrupts;

        //private HashSet<Sound> interruptedSounds =
        //    new HashSet<Sound>();

        ///// returns a float from 0.0 to 1.0 representing how much
        ///// of the sound has been played so far
        //public float Progress
        //{
        //    get
        //    {
        //        if (source == null || clip == null)
        //            return 0f;
        //        return (float)source.timeSamples / (float)clip.samples;
        //    }
        //}

        ///// returns true if the sound has finished playing
        ///// will always be false for looping sounds
        //public bool IsFinished
        //{
        //    get
        //    {
        //        return !loop && Progress >= 1f;
        //    }
        //}

        ///// returns true if the sound is currently playing,
        ///// false if it is paused or finished
        ///// can be set to true or false to play/pause the sound
        ///// will register the sound before playing
        //public bool IsPlaying
        //{
        //    get
        //    {
        //        return source != null && source.isPlaying;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            AudioManager.Instance.RegisterSound(this);
        //        }
        //        PlayOrPause(value, interrupts);
        //    }
        //}

        ///// Try to avoid calling this directly
        ///// Use AudioManager.NewSound instead
        //public Sound(string newName)
        //{
        //    name = newName;
        //    clip = (AudioClip)Resources.Load(name, typeof(AudioClip));
        //    if (clip == null)
        //        throw new Exception("Couldn't find AudioClip with name '" + name + "'. Are you sure the file is in a folder named 'Resources'?");
        //}

        //public void Update()
        //{
        //    if (source != null)
        //        source.loop = loop;
        //    if (IsFinished)
        //        Finish();
        //}

        ///// Try to avoid calling this directly
        ///// Use the Sound.playing property instead
        //public void PlayOrPause(bool play, bool pauseOthers)
        //{
        //    if (pauseOthers)
        //    {
        //        if (play)
        //        {
        //            interruptedSounds = new HashSet<Sound>();
        //            foreach (Sound sound in AudioManager.Instance.sounds)
        //            {
        //                if (sound.IsPlaying && sound != this)
        //                    interruptedSounds.Add(sound);
        //            }
        //        }
        //        foreach (Sound sound in interruptedSounds)
        //        {
        //            sound.PlayOrPause(!play, false);
        //        }
        //    }
        //    if (play && !source.isPlaying)
        //    {
        //        source.Play();
        //    }
        //    else
        //    {
        //        source.Pause();
        //    }
        //}

        ///// performs necessary actions when a sound finishes
        //public void Finish()
        //{
        //    PlayOrPause(false, true);
        //    if (callback != null)
        //        callback(this);
        //    MonoBehaviour.Destroy(source);
        //    source = null;
        //}

        ///// Reset the sound to its beginning
        //public void Reset()
        //{
        //    source.time = 0f;
        //}
    }
}