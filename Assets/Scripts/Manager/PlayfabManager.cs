using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BallDrop.Manager
{
    public enum PlayfabKeys
    {
        PlayerName,
        Level,
        ArcadeScore,
        ClassicScore,
        Coins,
        LastWatchedTime,
        TotalSeen,
        TrailData,
        SplatterData,
        SplatterCost,
        TrailCost,
    }

    public class PlayfabManager : SingletonMonoBehaviour<PlayfabManager>
    {

        private bool IsLoggedInToPlayfab = false;
        private LoginResult login_result = null;

        private void OnEnable()
        {
            MyEventManager.Instance.OnUnityAdsReady.AddListener(OnUnityAdsReady);
            MyEventManager.Instance.OnFacebookLoginComplete.AddListener(OnFacebookSDKLoginComplete);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.OnUnityAdsReady.RemoveListener(OnUnityAdsReady);
                MyEventManager.Instance.OnFacebookLoginComplete.RemoveListener(OnFacebookSDKLoginComplete);
            }
        }

        private void Start()
        {
            CheckConnection.Instance.CheckInternet(OnInternetAvailable, OnInternetNotAvailable);
        }

        #region Private methods
        private void OnInternetAvailable()
        {
            if (PreferenceManager.Instance.GetBoolPref(PrefKey.FacebookLogin))
                MyEventManager.Instance.LoginWithFacebook.Dispatch();
            else
            {
#if UNITY_EDITOR || UNITY_ANDROID
                LoginWithAndroidDevice();
#elif UNITY_IOS
               LoginWithIOSDevice();
#endif
            }
        }

        private void OnInternetNotAvailable()
        {
            MyEventManager.Instance.OnUserDataFetched.Dispatch(null);
        }

        private void OnUnityAdsReady()
        {
            if (IsLoggedIn())
            {
                GetDataFromPlayfab(new List<PlayfabKeys> { PlayfabKeys.TotalSeen }, res =>
                {
                    PlayerDataManager.Instance.UpdateTotalSeen(int.Parse(res.Data[PlayfabKeys.TotalSeen.ToString()].Value));
                    MyEventManager.Instance.OnGameRewardAdReady.Dispatch();
                }, err => { });
            }
        }

        private void LoginWithAndroidDevice()
        {
            Debug.unityLogger.Log(GameData.TAG, "Logging in with device");
            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
#if UNITY_EDITOR
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier + "TEST",
#else
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
#endif
                CreateAccount = true

            }, OnLoginWithDeviceSuccesfull, OnLoginWithDeviceError);
        }

        private void LoginWithIOSDevice()
        {
            PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true

            }, OnLoginWithDeviceSuccesfull, OnLoginWithDeviceError);
        }

        private void OnLoginWithDeviceError(PlayFabError playFabError)
        {
            Debug.unityLogger.Log(GameData.TAG, "Device Login Error - " + playFabError.Error + " --- " + playFabError.HttpCode + " --- " + playFabError.HttpStatus + " --- " + playFabError.ErrorMessage);
        }

        private void OnLoginWithDeviceSuccesfull(LoginResult loginResult)
        {
            Debug.unityLogger.Log(GameData.TAG, "Device login success");
            login_result = loginResult;
            if (!IsNewlyCreated() && !PreferenceManager.Instance.GetBoolPref(PrefKey.NotFirstGameOnline))
            {
                PreferenceManager.Instance.UpdateDictionaryPref(PrefKey.PlayfabDataUpdate, null);
                FetchDataOnLogin();
            }
            else
                UpdateAndGetData();
        }

        private void UpdateAndGetData()
        {
            Dictionary<PlayfabKeys, string> keyValuePairs = PreferenceManager.Instance.GetDictionaryPref(PrefKey.PlayfabDataUpdate);
            if (keyValuePairs != null && keyValuePairs.Count > 0)
            {
                Debug.unityLogger.Log(GameData.TAG, "Some data is to be updated - " + keyValuePairs.Count);
                foreach (PlayfabKeys key in keyValuePairs.Keys)
                {
                    Debug.unityLogger.Log(GameData.TAG, key + " = " + keyValuePairs[key]);
                }
                UpdatePlayfabUserData(keyValuePairs, result =>
                {
                    PreferenceManager.Instance.UpdateDictionaryPref(PrefKey.PlayfabDataUpdate, null);
                    FetchDataOnLogin();
                });
            }
            else
                FetchDataOnLogin();
        }

        private void OnFacebookSDKLoginComplete(FBData fBData)
        {
            Debug.unityLogger.Log(GameData.TAG, "fb login complete, syncining with playfab");
            if (PreferenceManager.Instance.GetBoolPref(PrefKey.FacebookLogin))
            {
                LoginWithFaceBook(fBData, UpdateAndGetData);
            }
            else
            {
                try
                {
                    PlayFabClientAPI.LinkFacebookAccount(new LinkFacebookAccountRequest()
                    {
                        AccessToken = fBData.accessToken.TokenString
                    },
                    OnLinkedWithFacebook =>
                    {
                        Debug.unityLogger.Log(GameData.TAG, "Linked with facebook");
                        PreferenceManager.Instance.UpdateBoolpref(PrefKey.FacebookLogin, true);
                        PlayerDataManager.Instance.UpdatePlayerName(fBData.name, UpdateAndGetData);
                    },
                    OnError =>
                    {
                        Debug.unityLogger.Log(GameData.TAG, "Error - " + OnError.Error);
                        if (OnError.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed)
                            LoginWithFaceBook(fBData, FetchDataOnLogin);
                        else
                        {
                            MySceneManager.Instance.HideLoadingCanvas();
                            MyEventManager.Instance.ShowMessage.Dispatch(GameStrings.FbErrorMessage + OnError.ErrorMessage);
                        }
                    });
                }
                catch (Exception)
                {
                    SaveUserData(new Dictionary<PlayfabKeys, string> { { PlayfabKeys.PlayerName, fBData.name } });
                    LoginWithFaceBook(fBData, UpdateAndGetData);
                }
            }
        }

        private void UnlinkFacebook()
        {
            PlayFabClientAPI.UnlinkFacebookAccount(new UnlinkFacebookAccountRequest(), result =>
            {
                print("unlink result = " + result.ToJson());
            }, error => { });
        }

        private void LoginWithFaceBook(FBData fBData, Action OnResult)
        {
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                AccessToken = fBData.accessToken.TokenString,
                CreateAccount = true
            },
            res =>
            {
                Debug.unityLogger.Log(GameData.TAG, "Logged in with facebook");
                login_result = res;
                PreferenceManager.Instance.UpdateBoolpref(PrefKey.FacebookLogin, true);
                OnResult();
            },
            err =>
            {
                MySceneManager.Instance.HideLoadingCanvas();
                Debug.unityLogger.Log(GameData.TAG, "Failed to Login to Facebook - " + err.Error + "\n" + err.ErrorMessage);
            });
        }

        private void FetchDataOnLogin()
        {
            Debug.unityLogger.Log(GameData.TAG, "Fetching data on login");
            #region FUTURE
            //List<PlayfabKeys> CostKeys = new List<PlayfabKeys>();
            //CostKeys.Add(PlayfabKeys.TrailCost);
            //CostKeys.Add(PlayfabKeys.SplatterCost);

            //GetTitleData(CostKeys,
            //    result =>
            //    {
            //        //Debug.Log("Got Title data - " + result.ToJson());
            //        MyEventManager.Instance.OnPricesFetched.Dispatch(result);
            //    },
            //    error =>
            //    {
            //        MyEventManager.Instance.OnPricesFetched.Dispatch(null);
            //    });
            #endregion

            GetDataFromPlayfab(null,
              result =>
              {
                  Debug.unityLogger.Log(GameData.TAG, "Playfab result--- " + result.ToJson());
                  PlayerData playerData = new PlayerData();

                  if (result.Data.ContainsKey(PlayfabKeys.PlayerName.ToString()))
                      playerData.Name = result.Data[PlayfabKeys.PlayerName.ToString()].Value;
                  else
                      playerData.Name = PreferenceManager.Instance.GetStringPref(PrefKey.PlayerName, "Guest");

                  if (!result.Data.ContainsKey(PlayfabKeys.Coins.ToString()))
                      playerData.Coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0);
                  else
                      playerData.Coins = int.Parse(result.Data[PlayfabKeys.Coins.ToString()].Value);

                  if (!result.Data.ContainsKey(PlayfabKeys.ArcadeScore.ToString()))
                      playerData.ArcadeScore = PreferenceManager.Instance.GetIntPref(PrefKey.BestScoreArcade, 0);
                  else
                      playerData.ArcadeScore = int.Parse(result.Data[PlayfabKeys.ArcadeScore.ToString()].Value);

                  if (!result.Data.ContainsKey(PlayfabKeys.ClassicScore.ToString()))
                      playerData.ClassicScore = PreferenceManager.Instance.GetIntPref(PrefKey.CurrentLevelScore, 0);
                  else
                      playerData.ClassicScore = int.Parse(result.Data[PlayfabKeys.ClassicScore.ToString()].Value);

                  if (!result.Data.ContainsKey(PlayfabKeys.Level.ToString()))
                      playerData.CurrentLevel = PreferenceManager.Instance.GetIntPref(PrefKey.PlayerLevel, 1);
                  else
                      playerData.CurrentLevel = int.Parse(result.Data[PlayfabKeys.Level.ToString()].Value);

                  if (!result.Data.ContainsKey(PlayfabKeys.TotalSeen.ToString()))
                      playerData.TotalSeen = 0;
                  else
                      playerData.TotalSeen = int.Parse(result.Data[PlayfabKeys.TotalSeen.ToString()].Value);

                  try
                  {
                      if (!result.Data.ContainsKey(PlayfabKeys.LastWatchedTime.ToString()))
                      {
                          playerData.LastWatchedTime = DateTime.ParseExact(PlayerData.MinDate, PlayerData.DTFormat, CultureInfo.InvariantCulture);
                      }
                      else
                      {
                          playerData.LastWatchedTime = DateTime.ParseExact(result.Data[PlayfabKeys.LastWatchedTime.ToString()].Value, PlayerData.DTFormat, CultureInfo.InvariantCulture);
                      }

                  }
                  catch (Exception e)
                  {
                      Debug.unityLogger.Log(GameData.TAG, e.Message);
                  }

                  Debug.unityLogger.Log(GameData.TAG, "Player data--- " + playerData.ToString());

                  MyEventManager.Instance.OnUserDataFetched.Dispatch(playerData);
                  MySceneManager.Instance.HideLoadingCanvas();
                  IsLoggedInToPlayfab = true;
                  CheckTime();
              },
              error =>
              {
                  MyEventManager.Instance.OnUserDataFetched.Dispatch(null);
              });
        }
        #endregion

        #region Public Methods
        public bool IsLoggedIn()
        {
            return IsLoggedInToPlayfab;
        }

        public DateTime? GetLastLoginTime()
        {
            if (login_result != null)
                return login_result.LastLoginTime;
            else return null;
        }

        public bool IsNewlyCreated()
        {
            if (login_result != null)
                return login_result.NewlyCreated;
            else
                return false;
        }

        public string GetPlayfabID()
        {
            if (login_result != null)
                return login_result.PlayFabId;
            else return GameStrings.EmptyString;
        }

        public void UpdateNameOnPlayfab(FBData fBData)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = fBData.name
            },
            OnNameUpdated =>
            {
                //Debug.Log("Name Updated Successfully");
            },
            OnNameUpdateError =>
            {
                Debug.Log("Name Update Error");
            });
        }

        public void UpdateDataOnGameEnd(GameMode gameMode, int score, int level)
        {
            PlayfabKeys gameKey = gameMode == GameMode.Arcade ? PlayfabKeys.ArcadeScore : PlayfabKeys.ClassicScore;
            Dictionary<PlayfabKeys, string> userData = new Dictionary<PlayfabKeys, string>
            {
                { gameKey, score.ToString()},
            };
            if (gameMode == GameMode.Classic)
                userData.Add(PlayfabKeys.Level, level.ToString());
            CheckConnection.Instance.CheckInternet(() =>
            {
                UpdatePlayfabUserData(userData, res =>
                {
                    Debug.unityLogger.Log(GameData.TAG, "Data updated - " + res.ToJson());
                    Dictionary<PlayfabKeys, string> saved = PreferenceManager.Instance.GetDictionaryPref(PrefKey.PlayfabDataUpdate);
                    if (saved != null)
                    {
                        if (saved.ContainsKey(gameKey))
                            saved.Remove(gameKey);
                        if (saved.ContainsKey(PlayfabKeys.Level) && gameKey == PlayfabKeys.ClassicScore)
                            saved.Remove(PlayfabKeys.Level);
                    }
                });
            }, () =>
            {
                SaveUserData(userData);
            });

        }

        public void SaveUserData(Dictionary<PlayfabKeys, string> data)
        {
            Dictionary<PlayfabKeys, string> saved = PreferenceManager.Instance.GetDictionaryPref(PrefKey.PlayfabDataUpdate);
            if (saved == null)
                saved = new Dictionary<PlayfabKeys, string>();
            foreach (PlayfabKeys key in data.Keys)
            {
                Debug.unityLogger.Log(GameData.TAG, "Data saved -> Key = " + key + " Value = " + data[key]);
                if (!saved.ContainsKey(key))
                    saved.Add(key, data[key]);
                else
                    saved[key] = data[key];
            }
            PreferenceManager.Instance.UpdateDictionaryPref(PrefKey.PlayfabDataUpdate, saved);
        }

        public void CheckTime()
        {
            PlayFabClientAPI.GetTime(new GetTimeRequest(), result =>
            {
                DateTime dt = PlayerDataManager.Instance.GetLastWatchedTime();
                if ((result.Time - dt).TotalDays >= 1 || ((result.Time - dt).TotalDays < 1 && result.Time.Day != dt.Day))
                {
                    UpdatePlayfabUserData(new Dictionary<PlayfabKeys, string> { { PlayfabKeys.LastWatchedTime, result.Time.ToString(PlayerData.DTFormat) } }, res =>
                    {
                        PlayerDataManager.Instance.UpdateLastWatchedTime(result.Time);
                        ResetTotalSeen();
                    });
                }
            }, error =>  { });
        }

        public void ResetTotalSeen()
        {
            UpdateTotalAdsSeen(0);
        }

        public void UpdateTotalSeen()
        {
            GetDataFromPlayfab(new List<PlayfabKeys> { PlayfabKeys.TotalSeen }, res =>
            {
                UpdateTotalAdsSeen(int.Parse(res.Data[PlayfabKeys.TotalSeen.ToString()].Value) + 1);
            }, err => { });
        }

        private void UpdateTotalAdsSeen(int total)
        {
            UpdatePlayfabUserData(new Dictionary<PlayfabKeys, string> { { PlayfabKeys.TotalSeen, total.ToString() } }, res =>
            {
                PlayerDataManager.Instance.UpdateTotalSeen(total);
            });
        }

        #region Generic Methods
        public void UpdatePlayfabUserData(Dictionary<PlayfabKeys, string> data, Action<UpdateUserDataResult> OnUpdated)
        {
            if (login_result != null)
            {
                if (data != null)
                {
                    Dictionary<string, string> updateData = new Dictionary<string, string>();
                    foreach (KeyValuePair<PlayfabKeys, string> keyValue in data)
                    {
                        updateData.Add(keyValue.Key.ToString(), keyValue.Value.ToString());
                    }
                    UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
                    {
                        Data = updateData,
                        Permission = UserDataPermission.Public
                    };
                    PlayFabClientAPI.UpdateUserData(userDataRequest, OnUpdated, error =>
                    {
                        Debug.unityLogger.Log(GameData.TAG, error);
                        Debug.unityLogger.Log(GameData.TAG, error.ErrorMessage);
                    });
                }
            }
            else
            {
                SaveUserData(data);
            }
        }

        public void GetDataFromPlayfab(List<PlayfabKeys> keys, Action<GetUserDataResult> OnResult,
                                        Action<PlayFabError> OnError)
        {
            GetUserDataRequest request = new GetUserDataRequest
            {
                Keys = new List<string>()
            };

            if (keys != null)
            {
                foreach (PlayfabKeys key in keys)
                {
                    request.Keys.Add(key.ToString());
                }
            }
            PlayFabClientAPI.GetUserData(request, OnResult, OnError);
        }

        public void GetTitleData(List<PlayfabKeys> TitleKeys, Action<GetTitleDataResult> Result,
                                        Action<PlayFabError> OnError)
        {
            List<string> keyList = new List<string>();
            if (TitleKeys != null)
            {
                foreach (PlayfabKeys key in TitleKeys)
                {
                    keyList.Add(key.ToString());
                }
            }

            GetTitleDataRequest request = new GetTitleDataRequest()
            {
                Keys = keyList
            };
            PlayFabClientAPI.GetTitleData(request, Result, OnError);

        }

        #endregion

        #endregion

    }
}