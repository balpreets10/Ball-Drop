﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Manager
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        //  public HashSet<Sound> sounds =
        //new HashSet<Sound>();

        //  /// Creates a new sound, registers it, gives it the properties specified, and starts playing it
        //  public Sound PlayNewSound(string soundName, bool loop = false, bool interrupts = false, Action<Sound> callback = null)
        //  {
        //      Sound sound = NewSound(soundName, loop, interrupts, callback);
        //      sound.IsPlaying = true;
        //      return sound;
        //  }

        //  /// Creates a new sound, registers it, and gives it the properties specified
        //  public Sound NewSound(string soundName, bool loop = false, bool interrupts = false, Action<Sound> callback = null)
        //  {
        //      Sound sound = new Sound(soundName);
        //      RegisterSound(sound);
        //      sound.loop = loop;
        //      sound.interrupts = interrupts;
        //      sound.callback = callback;
        //      return sound;
        //  }

        //  /// Registers a sound with the AudioManager and gives it an AudioSource if necessary
        //  /// You should probably avoid calling this function directly and just use
        //  /// NewSound and PlayNewSound instead
        //  public void RegisterSound(Sound sound)
        //  {
        //      sounds.Add(sound);
        //      if (sound.source == null)
        //      {
        //          AudioSource source = gameObject.AddComponent<AudioSource>();
        //          source.clip = sound.clip;
        //          sound.source = source;
        //      }
        //  }

        //  private void Update()
        //  {
        //      foreach (Sound sound in sounds)
        //      {
        //          sound.Update();
        //      }
        //  }
    }
}