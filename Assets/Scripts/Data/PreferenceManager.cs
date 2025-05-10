using BallDrop.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public enum PrefKey
    {
        //string
        PlayerName,
        ProfilePic,

        //int
        PlayerLevel,
        Coins,
        BestScoreArcade,
        PreviousScoreArcade,
        CurrentLevelScore,
        GamesPlayed,

        //float
        Music,
        Effect,
        MovementSensitivity,

        //bool
        FacebookLogin,
        NotFirstGameOnline,

        //custom
        PlayfabDataUpdate,
        TrailData,
        SplatterData,
        TrailCost,
        SplatterCost
    }

    public class PreferenceManager : SingletonMonoBehaviour<PreferenceManager>
    {

        public void UpdateBoolpref(PrefKey Pref, bool value)
        {
            PlayerPrefs.SetInt(Pref.ToString(), (value ? 1 : 0));
        }

        public bool GetBoolPref(PrefKey Pref)
        {
            return (PlayerPrefs.GetInt(Pref.ToString()) == 1 ? true : false);
        }

        public void UpdateIntPref(PrefKey Pref, int value)
        {
            PlayerPrefs.SetInt(Pref.ToString(), value);
        }

        public int GetIntPref(PrefKey Pref, int Default)
        {
            return PlayerPrefs.GetInt(Pref.ToString(), Default);
        }

        public void IncrementIntPref(PrefKey Pref, int Default)
        {
            int temp = GetIntPref(Pref, Default);
            UpdateIntPref(Pref, ++temp);
        }

        public void UpdateFloatPref(PrefKey Pref, float value)
        {
            PlayerPrefs.SetFloat(Pref.ToString(), value);
        }

        public float GetFloatPref(PrefKey Pref, float Default)
        {
            return PlayerPrefs.GetFloat(Pref.ToString(), Default);
        }

        public void UpdateStringPref(PrefKey Pref, string value)
        {
            PlayerPrefs.SetString(Pref.ToString(), value);
        }

        public string GetStringPref(PrefKey Pref, string Default)
        {
            return PlayerPrefs.GetString(Pref.ToString(), Default);
        }


        //---------------Custom prefernces---------------------
        public void UpdateCustomPref(PrefKey key, object obj)
        {
            string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key.ToString(), json);
        }

        public object GetCustomPref(PrefKey key, Type type)
        {
            string json = PlayerPrefs.GetString(key.ToString());
             if (string.IsNullOrEmpty(json))
                return null;
            return JsonUtility.FromJson(json, type);
        }

        public void UpdateDictionaryPref(PrefKey key, Dictionary<PlayfabKeys, string> keyValues)
        {
            if (keyValues != null)
            {
                DictionaryHelper dictionary = new DictionaryHelper(keyValues);
                UpdateCustomPref(key, dictionary);
            }
            else
            {
                UpdateCustomPref(key, null);
            }
        }

        public Dictionary<PlayfabKeys, string> GetDictionaryPref(PrefKey key)
        {
            Dictionary<PlayfabKeys, string> d = new Dictionary<PlayfabKeys, string>();
            DictionaryHelper dictionary = (DictionaryHelper)GetCustomPref(key, typeof(DictionaryHelper));
            if (dictionary == null)
                return null;
            for (int i = 0; i < dictionary.keys.Count; i++)
            {
                d.Add(dictionary.keys[i], dictionary.values[i]);
            }
            return d;
        }
    }

    [Serializable]
    class DictionaryHelper
    {
        public List<PlayfabKeys> keys;
        public List<string> values;

        public DictionaryHelper(Dictionary<PlayfabKeys, string> keyValues)
        {
            keys = new List<PlayfabKeys>();
            values = new List<string>();
            foreach (PlayfabKeys key in keyValues.Keys)
            {
                keys.Add(key);
                values.Add(keyValues[key]);
            }
        }
    }
}