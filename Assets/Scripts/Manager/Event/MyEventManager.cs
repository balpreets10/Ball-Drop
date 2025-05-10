using BallDrop.Interfaces;
using UnityEngine;

namespace BallDrop
{
    public static class MyEventManager
    {
        public static MyEvent OnObjectsInstantiated = new MyEvent();
        public static MyEvent DeactivatePooledObjects = new MyEvent();

       public static MyEvent<float> OnMusicVolumeChanged = new MyEvent<float>();
        public static MyEvent<float> OnEffectVolumeChanged = new MyEvent<float>();
        public static MyEvent<int> OnScoreUpdated = new MyEvent<int>(); // increment that happened
        public static MyEvent<int> OnDynamicScoreDeactivated = new MyEvent<int>();// the increment that happened
        public static MyEvent<int> LoadLevelLeaderboard = new MyEvent<int>();
        public static MyEvent<int> ScrollToMenu = new MyEvent<int>();
        public static MyEvent OnLevelCompleted = new MyEvent();
        public static MyEvent OnPlayerDeath = new MyEvent();
        public static MyEvent ChangeTexture = new MyEvent();
        public static MyEvent StartGame = new MyEvent();
        public static MyEvent OnPlayerActivated = new MyEvent();
        public static MyEvent PauseGame = new MyEvent();
        public static MyEvent EndGame = new MyEvent();
        public static MyEvent Reveal = new MyEvent();
        public static MyEvent<float, float> OnLandedOnXCube = new MyEvent<float, float>(); //Duration,ScaleFactor
        public static MyEvent<float> OnLandedOnReverseCube = new MyEvent<float>();
        public static MyEvent<Texture> OnBackgroundUpdated = new MyEvent<Texture>();
        public static MyEvent OnDeathAnimationFinish = new MyEvent();
        public static MyEvent QuitGame = new MyEvent();

        //Rows
        public static MyEvent SpawnRow = new MyEvent();
        public static MyEvent<IRow> RemoveRow = new MyEvent<IRow>();
        public static MyEvent OnRowsSpawned = new MyEvent();
        public static MyEvent OnRowPassed = new MyEvent();
        //public static MyEvent StartSpawningRows = new MyEvent();

        //Data
        public static MyEvent<GameMode> SetGameMode = new MyEvent<GameMode>();

        //LOGIN
        public static MyEvent LoginWithGoogle = new MyEvent();
        public static MyEvent LoginWithFacebook = new MyEvent();
        public static MyEvent<FBData> OnFacebookLoginComplete = new MyEvent<FBData>();

        //Currency
        public static MyEvent UpdateCoins = new MyEvent();
        public static MyEvent<int> OnCoinsUpdated = new MyEvent<int>();
        public static MyEvent<int> OnCoinsAwarded = new MyEvent<int>();

        //Cubes
        public static MyEvent<bool> OnDirectionSwitched = new MyEvent<bool>();

        //Powerups
        public static MyEvent<float> OnLineGuideCollected = new MyEvent<float>();
        public static MyEvent<float> OnSlowDownCollected = new MyEvent<float>();
        public static MyEvent<float> OnShieldCollected = new MyEvent<float>();

        //Playfab
        public static MyEvent LoginWithPlayfab = new MyEvent();
        public static MyEvent<PlayerData> OnUserDataFetched = new MyEvent<PlayerData>();
        //public MyEvent<GetTitleDataResult> OnPricesFetched = new MyEvent<GetTitleDataResult>();
        public static MyEvent UpdateUI = new MyEvent();

        //Popups
        public static MyEvent<string> ShowMessage = new MyEvent<string>();
        public static MyEvent GetPlayerName = new MyEvent();

        //Ad
        public static MyEvent OnGameRewardAdReady = new MyEvent();
        public static MyEvent<int> OnCompletedAwardAd = new MyEvent<int>();
        public static MyEvent OnUnityAdsReady = new MyEvent();

        //Player revival
        public static MyEvent ReviveOption = new MyEvent();
        public static MyEvent OnCancelledRevive = new MyEvent();
        public static MyEvent OnCompletedRevivalAd = new MyEvent();
        public static MyEvent<GameObject> SetPlayerPosAfterRevival = new MyEvent<GameObject>();

        //ScrollItems
        public static MyEvent<int, ItemType> UpdateScrollItems = new MyEvent<int, ItemType>();
        public static MyEvent<int, ItemType, LockStatus> UpdateLockStatus = new MyEvent<int, ItemType, LockStatus>();
        public static MyEvent<Texture> OnSplatterSelected = new MyEvent<Texture>();
        public static MyEvent<int, Texture> OnTrailChanged = new MyEvent<int, Texture>();

    }
}