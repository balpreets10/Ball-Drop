using BallDrop.Interfaces;
using UnityEngine;

namespace BallDrop
{
    public class MyEventManager : SingletonMonoBehaviour<MyEventManager>
    {
        public MyEvent OnObjectsInstantiated = new MyEvent();
        public MyEvent DeactivatePooledObjects = new MyEvent();

       public MyEvent<float> OnMusicVolumeChanged = new MyEvent<float>();
        public MyEvent<float> OnEffectVolumeChanged = new MyEvent<float>();
        public MyEvent<int> OnScoreUpdated = new MyEvent<int>(); // increment that happened
        public MyEvent<int> OnDynamicScoreDeactivated = new MyEvent<int>();// the increment that happened       
        public MyEvent<int> LoadLevelLeaderboard = new MyEvent<int>();
        public MyEvent<int> ScrollToMenu = new MyEvent<int>();
        public MyEvent OnLevelCompleted = new MyEvent();
        public MyEvent OnPlayerDeath = new MyEvent();
        public MyEvent ChangeTexture = new MyEvent();
        public MyEvent StartGame = new MyEvent();
        public MyEvent OnPlayerActivated = new MyEvent();
        public MyEvent PauseGame = new MyEvent();
        public MyEvent EndGame = new MyEvent();
        public MyEvent Reveal = new MyEvent();
        public MyEvent<float, float> OnLandedOnXCube = new MyEvent<float, float>(); //Duration,ScaleFactor
        public MyEvent<float> OnLandedOnReverseCube = new MyEvent<float>();
        public MyEvent<Texture> OnBackgroundUpdated = new MyEvent<Texture>();
        public MyEvent OnDeathAnimationFinish = new MyEvent();
        public MyEvent QuitGame = new MyEvent();
        
        //Rows
        public MyEvent SpawnRow = new MyEvent();
        public MyEvent<IRow> RemoveRow = new MyEvent<IRow>();
        public MyEvent OnRowsSpawned = new MyEvent();
        public MyEvent OnRowPassed = new MyEvent();
        //public MyEvent StartSpawningRows = new MyEvent();

        //Data
        public MyEvent<GameMode> SetGameMode = new MyEvent<GameMode>();
        
        //LOGIN
        public MyEvent LoginWithGoogle = new MyEvent();
        public MyEvent LoginWithFacebook = new MyEvent();
        public MyEvent<FBData> OnFacebookLoginComplete = new MyEvent<FBData>();

        //Currency
        public MyEvent UpdateCoins = new MyEvent();
        public MyEvent<int> OnCoinsUpdated = new MyEvent<int>();
        public MyEvent<int> OnCoinsAwarded = new MyEvent<int>();

        //Cubes
        public MyEvent<bool> OnDirectionSwitched = new MyEvent<bool>();

        //Powerups
        public MyEvent<float> OnLineGuideCollected = new MyEvent<float>();
        public MyEvent<float> OnSlowDownCollected = new MyEvent<float>();
        public MyEvent<float> OnShieldCollected = new MyEvent<float>();

        //Playfab
        public MyEvent LoginWithPlayfab = new MyEvent();
        public MyEvent<PlayerData> OnUserDataFetched = new MyEvent<PlayerData>();
        //public MyEvent<GetTitleDataResult> OnPricesFetched = new MyEvent<GetTitleDataResult>();
        public MyEvent UpdateUI = new MyEvent();

        //Popups
        public MyEvent<string> ShowMessage = new MyEvent<string>();
        public MyEvent GetPlayerName = new MyEvent();

        //Ad
        public MyEvent OnGameRewardAdReady = new MyEvent();
        public MyEvent<int> OnCompletedAwardAd = new MyEvent<int>();
        public MyEvent OnUnityAdsReady = new MyEvent();

        //Player revival
        public MyEvent ReviveOption = new MyEvent();
        public MyEvent OnCancelledRevive = new MyEvent();
        public MyEvent OnCompletedRevivalAd = new MyEvent();
        public MyEvent<GameObject> SetPlayerPosAfterRevival = new MyEvent<GameObject>();

        //ScrollItems
        public MyEvent<int, ItemType> UpdateScrollItems = new MyEvent<int, ItemType>();
        public MyEvent<int, ItemType, LockStatus> UpdateLockStatus = new MyEvent<int, ItemType, LockStatus>();
        public MyEvent<Texture> OnSplatterSelected = new MyEvent<Texture>();
        public MyEvent<int, Texture> OnTrailChanged = new MyEvent<int, Texture>();

    }
}