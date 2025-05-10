using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace BallDrop
{
    public class UnityAdManager : SingletonMonoBehaviour<UnityAdManager>// IUnityAdsListener
    {
        private readonly string gameId = "3173825";
        private readonly string bannerPlacementId = "BannerPromotions";
        private readonly string LifeRewardId = "rewardedVideo";
        private readonly string GameRewardsId = "GameRewards";

        private bool testMode = true;

        private bool ShownOnceInAGame = false;

        [SerializeField]
        public int MaxPerDay;

        private void OnEnable()
        {
            MyEventManager.EndGame.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
                MyEventManager.EndGame.RemoveListener(OnGameEnd);
        }

        private void Start()
        {
            //Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }

        public void ShowBanner(BannerPosition position)
        {
            StartCoroutine(ShowBannerWhenReady(position));
        }

        private IEnumerator ShowBannerWhenReady(BannerPosition position)
        {
            // while (!Advertisement.IsReady(bannerPlacementId))
            // {
            //     yield return new WaitForSeconds(0.5f);
            // }
            // Advertisement.Banner.SetPosition(position);
            // Advertisement.Banner.Show(bannerPlacementId);
            yield return null;
        }

        public void ShowInterstitial()
        {
            //Advertisement.Show();
        }

        public void ShowGameRewardVideo()
        {
            Advertisement.Show(GameRewardsId);
        }

        public void ShowLifeRewardVideo()
        {
            Advertisement.Show(LifeRewardId);
        }

        public bool CheckGameRewardReady()
        {
            return false;
            //return (Advertisement.IsReady(GameRewardsId) && PlayerDataManager.Instance.GetTotalAdsSeen() < MaxPerDay);
        }

        public bool CheckLifeRewardReady()
        {
            return false;
            //return (Advertisement.IsReady(LifeRewardId) && !ShownOnceInAGame && PlayerDataManager.Instance.GetTotalAdsSeen() < MaxPerDay);
        }

        public bool CheckInterstitialReady()
        {
            return false;
            //return Advertisement.IsReady("video");
        }

        public void HideBanner()
        {
            if (Advertisement.Banner.isLoaded)
                Advertisement.Banner.Hide();
        }

        private void OnGameEnd()
        {
            ShownOnceInAGame = false;
        }

        public static long ElapsedTime()
        {
            if (Application.platform != RuntimePlatform.Android)
                return 0;
            AndroidJavaClass systemClock = new AndroidJavaClass("android.os.SystemClock");
            return systemClock.CallStatic<long>("elapsedRealtime");
        }

        //-----------------------------CALLBACKS--------------------------------------
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (showResult == ShowResult.Finished)
            {
                PlayfabManager.Instance.UpdateTotalSeen();
                if (placementId == LifeRewardId)
                {
                    ShownOnceInAGame = true;
                    MyEventManager.OnCompletedRevivalAd.Dispatch();
                }
                if (placementId == GameRewardsId)
                {
                    MyEventManager.OnCompletedAwardAd.Dispatch(UnityEngine.Random.Range(5, 15));
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                Debug.Log("User Skipped Ad");
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.Log("The ad did not finish due to an error");
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            if (placementId == GameRewardsId)
            {
                MyEventManager.OnUnityAdsReady.Dispatch();
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.unityLogger.Log(GameData.TAG, message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
        }
    }
}