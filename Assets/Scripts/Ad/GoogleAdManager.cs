using UnityEngine;
//using GoogleMobileAds.Api;
using System;

namespace BallDrop
{
    public class GoogleAdManager : SingletonMonoBehaviour<GoogleAdManager>
    {
        /*private RewardBasedVideoAd rewardBasedVideo;
        private BannerView bannerView;
        private InterstitialAd interstitial;

        public void Start()
        {
#if UNITY_ANDROID
            string appId = "ca-app-pub-8120011900374355~3607795101";
#else
            string appId = "unexpected_platform";
#endif

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(appId);

            rewardBasedVideo = RewardBasedVideoAd.Instance;

            rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
            rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder().Build();
        }

        public void DestroyBannerView()
        {
            bannerView.Destroy();
        }

        public void DestroyInterstitial()
        {
            interstitial.Destroy();
        }

        /*-------------------------------------------BANNER REQUEST AND  CALLBACKS---------------------------------------------------------*/

       /* #region Banner Ad Requests and Callbacks

        public void RequestBanner()
        {
#if UNITY_ANDROID
            string adUnitId;
            if (GameData.Instance.buildType == BuildType.Test)
                adUnitId = "ca-app-pub-3940256099942544/6300978111";
            else
                adUnitId = "ca-app-pub-8120011900374355/5659243378";
#elif UNITY_EDITOR
            string adUnitId = "unused";
#else
            string adUnitId = "unexpected_platform";
#endif

            // Clean up banner ad before creating a new one.
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            // Create a 320x50 banner at the bottom of the screen.
            bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            bannerView.OnAdLoaded += HandleAdLoaded;
            bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
            bannerView.OnAdOpening += HandleAdOpened;
            bannerView.OnAdClosed += HandleAdClosed;
            bannerView.OnAdLeavingApplication += HandleAdLeftApplication;

            bannerView.LoadAd(CreateAdRequest());
        }

        private void HandleAdLoaded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleAdOpened(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleAdClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleAdLeftApplication(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion Banner Ad Requests and Callbacks

        /*-------------------------------------------INTERSTITIAL AD REQUEST AND  CALLBACKS---------------------------------------------------------*/

        /*#region Interstitial Requests and Callbacks

        public void RequestInterstitial()
        {
#if UNITY_ANDROID
            string adUnitId;
            if (GameData.Instance.buildType == BuildType.Test)
                adUnitId = "ca-app-pub-3940256099942544/1033173712";
            else
                adUnitId = "ca-app-pub-8120011900374355/3033080035";
#elif UNITY_EDITOR
            string adUnitId = "unused";
#else
        string adUnitId = "unexpected_platform";
#endif

            // Clean up interstitial ad before creating a new one.
            if (interstitial != null)
            {
                interstitial.Destroy();
            }

            interstitial = new InterstitialAd(adUnitId);

            interstitial.OnAdLoaded += HandleInterstitialLoaded;
            interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
            interstitial.OnAdOpening += HandleInterstitialOpened;
            interstitial.OnAdClosed += HandleInterstitialClosed;
            interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;

            interstitial.LoadAd(CreateAdRequest());
        }

        public void ShowInterstitial()
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
            else
            {
                Debug.Log("Interstitial ad is not ready yet");
            }
        }

        private void HandleInterstitialLoaded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleInterstitialOpened(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleInterstitialClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleInterstitialLeftApplication(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion Interstitial Requests and Callbacks

        /*-------------------------------------------REWARD BASED AD REQUEST AND  CALLBACKS---------------------------------------------------------*/

        /*#region Reward Requests and Callbacks

        public void RequestRewardBasedVideo()
        {
#if UNITY_ANDROID
            string adUnitId;
            if (GameData.Instance.buildType == BuildType.Test)
                adUnitId = "ca-app-pub-3940256099942544/5224354917";
            else
                adUnitId = "ca-app-pub-8120011900374355/9406916697";
#elif UNITY_EDITOR
            string adUnitId = "unused";
#else
        string adUnitId = "unexpected_platform";
#endif

            rewardBasedVideo.LoadAd(CreateAdRequest(), adUnitId);
        }

        public void ShowRewardBasedVideo()
        {
            if (rewardBasedVideo.IsLoaded())
            {
                rewardBasedVideo.Show();
            }
            else
            {
                Debug.Log("Reward based video ad is not ready yet");
            }
        }

        private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
        {
        }

        private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
        {
        }

        private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
        }

        private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
        {
        }

        private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
        {
        }

        private void HandleRewardBasedVideoRewarded(object sender, Reward e)
        {
            string type = e.Type;
            double amount = e.Amount;
            print("User rewarded with: " + amount.ToString() + " " + type);
        }

        private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
        {
        }

        #endregion Reward Requests and Callbacks
        */
    }
}