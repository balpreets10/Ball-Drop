using BallDrop.Manager;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class PlayerDataManager : SingletonMonoBehaviour<PlayerDataManager>
    {
        private PlayerData playerdata;
        public bool nameSet = false;

        #region To be used later
        //public TrailCost TrailCostList;
        //public SplatterCost SplatterCostList;
        #endregion

        private void OnEnable()
        {
            MyEventManager.Instance.OnUserDataFetched.AddListener(OnUserDataFetched);
            MyEventManager.Instance.OnCoinsAwarded.AddListener(UpdateCoins);
            MyEventManager.Instance.OnCoinsUpdated.AddListener(UpdateCoins);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.OnUserDataFetched.RemoveListener(OnUserDataFetched);
                MyEventManager.Instance.OnCoinsAwarded.RemoveListener(UpdateCoins);
                MyEventManager.Instance.OnCoinsUpdated.RemoveListener(UpdateCoins);
            }
        }

        private void Start()
        {
            playerdata = new PlayerData()
            {
                Coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0),
                CurrentLevel = PreferenceManager.Instance.GetIntPref(PrefKey.PlayerLevel, 1),
                ArcadeScore = PreferenceManager.Instance.GetIntPref(PrefKey.BestScoreArcade, 0),
                ClassicScore = PreferenceManager.Instance.GetIntPref(PrefKey.CurrentLevelScore, 0),
                Name = PreferenceManager.Instance.GetStringPref(PrefKey.PlayerName, "Guest"),
                TotalSeen = UnityAdManager.Instance.MaxPerDay
            };
        }

        private void OnUserDataFetched(PlayerData data)
        {
            if (data != null)
            {
                playerdata = data;
                CoinManager.Instance.RefreshCoins(data.Coins);
                UpdatePreferences();
                MyEventManager.Instance.UpdateUI.Dispatch();
                Debug.Log("Updated UI");
            }

            if (data == null)
            {
                playerdata = new PlayerData()
                {
                    Name = "Unknown",
                    Coins = 0,
                    CurrentLevel = 1,
                };
                CoinManager.Instance.RefreshCoins(data.Coins);
                UpdatePreferences();
                MyEventManager.Instance.UpdateUI.Dispatch();
            }
        }

        private void UpdateCoins(int coins)
        {
            playerdata.Coins = coins;
            Dictionary<PlayfabKeys, string> keyValuePairs = new Dictionary<PlayfabKeys, string>
            {
                    { PlayfabKeys.Coins, playerdata.Coins.ToString() }
            };
            CheckConnection.Instance.CheckInternet(() =>
            {
                PlayfabManager.Instance.UpdatePlayfabUserData(keyValuePairs, result =>
                {
                    Debug.unityLogger.Log(GameData.TAG, "Coins updated = " + result.ToJson());
                    Dictionary<PlayfabKeys, string> saved = PreferenceManager.Instance.GetDictionaryPref(PrefKey.PlayfabDataUpdate);
                    if (saved != null && saved.ContainsKey(PlayfabKeys.Coins))
                        saved.Remove(PlayfabKeys.Coins);
                });
            }, () =>
            {
                PlayfabManager.Instance.SaveUserData(keyValuePairs);
            });
        }

        private void UpdatePreferences()
        {
            PreferenceManager.Instance.UpdateStringPref(PrefKey.PlayerName, playerdata.Name);
            PreferenceManager.Instance.UpdateIntPref(PrefKey.PlayerLevel, playerdata.CurrentLevel);
            PreferenceManager.Instance.UpdateIntPref(PrefKey.BestScoreArcade, playerdata.ArcadeScore);
            PreferenceManager.Instance.UpdateIntPref(PrefKey.CurrentLevelScore, playerdata.ClassicScore);
            //PreferenceManager.Instance.UpdateCustomPref(PrefKey.TrailData, playerdata.trailData);
            //PreferenceManager.Instance.UpdateCustomPref(PrefKey.SplatterData, playerdata.splatterData);
        }

        public void UpdateDataOnGameEnd(GameMode gameMode, int score, bool levelCleared)
        {
            int s;
            if (gameMode == GameMode.Arcade)
            {
                PreferenceManager.Instance.UpdateIntPref(PrefKey.PreviousScoreArcade, score);
                if (playerdata.ArcadeScore < score)
                {
                    playerdata.ArcadeScore = score;
                }
                s = playerdata.ArcadeScore;
            }
            else
            {
                if (levelCleared)
                {
                    playerdata.CurrentLevel++;
                    playerdata.ClassicScore = score;
                }
                s = playerdata.ClassicScore;
            }
            UpdatePreferences();
            try
            {
                PlayfabManager.Instance.UpdateDataOnGameEnd(gameMode, s, playerdata.CurrentLevel);
            }
            catch (Exception) { }
        }

        public void UpdatePlayerName(string name, Action onResult = null)
        {
            playerdata.Name = name;
            PlayfabManager.Instance.UpdatePlayfabUserData(new Dictionary<PlayfabKeys, string> { { PlayfabKeys.PlayerName, name } }, result =>
            {
                Debug.unityLogger.Log(GameData.TAG, result.ToJson());
                onResult?.Invoke();
            });
        }

        public void UpdateLastWatchedTime(DateTime dateTime)
        {
            playerdata.LastWatchedTime = dateTime;
        }

        public void UpdateTotalSeen(int total)
        {
            playerdata.TotalSeen = total;
        }

        //For Testing only
        public void SetLevel(int level)
        {
            playerdata.CurrentLevel = level;
        }

        public string GetPlayerName()
        {
            return playerdata.Name;
        }

        public int GetPlayerLevel()
        {
            return playerdata.CurrentLevel;
        }

        public int GetClassicScore()
        {
            return playerdata.ClassicScore;
        }

        public int GetArcadeScore()
        {
            return playerdata.ArcadeScore;
        }

        public int GetCoins()
        {
            return playerdata.Coins;
        }

        public int GetTotalAdsSeen()
        {
            return playerdata.TotalSeen;
        }

        public DateTime GetLastWatchedTime()
        {
            return playerdata.LastWatchedTime;
        }
    }

    #region Future Updates
    //[Serializable]
    //public class TrailCost
    //{
    //    public List<int> trailCost;

    //    public TrailCost()
    //    {
    //        trailCost = new List<int>() { 0, 100, 200, 300, 400, 500 };

    //    }
    //}

    //[Serializable]
    //public class SplatterCost
    //{
    //    public List<int> splatterCost;

    //    public SplatterCost()
    //    {
    //        splatterCost = new List<int>() { 0, 100, 150, 200, 250, 300, 400, 500, 600, 700, 700, 700, 700, 700, 700, 700, 700, 700, 700, 700 };
    //    }
    //}

    //private void OnPricesFetched(GetTitleDataResult result)
    //{
    //    if (result != null)
    //    {
    //        TrailCostList = JsonUtility.FromJson<TrailCost>(result.Data[PlayfabKeys.TrailCost.ToString()]);
    //        SplatterCostList = JsonUtility.FromJson<SplatterCost>(result.Data[PlayfabKeys.SplatterCost.ToString()]);
    //        PreferenceManager.Instance.UpdateCustomPref(PrefKey.TrailCost, TrailCostList);
    //        PreferenceManager.Instance.UpdateCustomPref(PrefKey.SplatterCost, SplatterCostList);

    //    }
    //    else
    //    {
    //        TrailCost tempCost = (TrailCost)PreferenceManager.Instance.GetCustomPref(PrefKey.TrailCost, typeof(TrailCost));
    //        if (tempCost != null)
    //        {
    //            TrailCostList = tempCost;
    //            SplatterCostList = (SplatterCost)PreferenceManager.Instance.GetCustomPref(PrefKey.SplatterCost, typeof(SplatterCost));
    //        }
    //        else
    //        {
    //            TrailCostList = new TrailCost();
    //            SplatterCostList = new SplatterCost();
    //            PreferenceManager.Instance.UpdateCustomPref(PrefKey.TrailCost, TrailCostList);
    //            PreferenceManager.Instance.UpdateCustomPref(PrefKey.SplatterCost, SplatterCostList);

    //        }
    //    }
    //}
    #endregion


}