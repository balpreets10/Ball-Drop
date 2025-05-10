//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace BallDrop
{
    public class GooglePlayManager : SingletonMonoBehaviour<GooglePlayManager>
    {
        //    private readonly string DefaultLeaderboard = "CgkI_YSshJYFEAIQBg";
        //    public PlayerData playerData = new PlayerData();

        //    public GooglePlayData googlePlayData = new GooglePlayData();

        //    private void Start()
        //    {
        //        ActivatePlayGames();
        //        if (PreferenceManager.Instance.GetBoolPref(PrefKey.GooglePlayLogin))
        //            LoginWithGoogle();

        //    }

        //    void ActivatePlayGames()
        //    {
        //        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //        .EnableSavedGames()
        //        //// registers a callback to handle game invitations received while the game is not running.
        //        ////.WithInvitationDelegate(< callback method >)
        //        //// requests the email address of the player be available.
        //        //// Will bring up a prompt for consent.
        //        .RequestEmail()

        //        //// requests a server auth code be generated so it can be passed to an
        //        ////  associated back end server application and exchanged for an OAuth token.
        //        .RequestServerAuthCode(false)
        //        //// requests an ID token be generated.  This OAuth token can be used to
        //        ////  identify the player to other services such as Firebase.
        //        .RequestIdToken()
        //        .Build();

        //        PlayGamesPlatform.InitializeInstance(config);
        //        // recommended for debugging:
        //        PlayGamesPlatform.DebugLogEnabled = true;
        //        // Activate the Google Play Games platform
        //        PlayGamesPlatform platform = PlayGamesPlatform.Activate();
        //        if (platform == null)
        //            Debug.Log("Null Platform");
        //        else
        //        {
        //            Debug.Log("Platform activated-" + platform.GetType());

        //        }
        //    }

        //    private void OnEnable()
        //    {
        //        MyEventManager.Instance.LoginWithGoogle.AddListener(LoginWithGoogle);
        //    }

        //    private void OnDisable()
        //    {
        //        if (MyEventManager.Instance != null)
        //        {
        //            MyEventManager.Instance.LoginWithGoogle.RemoveListener(LoginWithGoogle);
        //        }
        //    }

        //    private void LoginWithGoogle()
        //    {
        //        Debug.Log("Showing Google Auth");
        //        Social.localUser.Authenticate(OnAuthenticationComplete);
        //    }

        //    private void OnAuthenticationComplete(bool result, string message)
        //    {
        //        Debug.Log("Result - " + result + "\nMessage - " + message);
        //        if (result)
        //        {
        //            PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, onAuthCodeGenerated);

        //        }
        //    }

        //    private void onAuthCodeGenerated(string code)
        //    {
        //        Debug.Log("Code Generated");
        //        googlePlayData.ServerAuthCode = code;
        //        Debug.Log("Player ID - " + PlayGamesPlatform.Instance.localUser.id);
        //        Debug.Log("Player Name - " + PlayGamesPlatform.Instance.localUser.userName);
        //        Debug.Log("Auth Code - " + code);
        //        playerData.name = PlayGamesPlatform.Instance.localUser.userName;
        //        MyEventManager.Instance.OnGoogleLoginComplete.Dispatch(playerData, googlePlayData);
        //        PreferenceManager.Instance.UpdateBoolpref(PrefKey.GooglePlayLogin, true);
        //    }

        //    public void UpdateScore(int Score)
        //    {
        //        Social.ReportScore(Score, DefaultLeaderboard, OnScoreUpdated);
        //    }

        //    private void OnScoreUpdated(bool result)
        //    {
        //        if (result)
        //            Debug.Log("Score Updated Succesfully");
        //        else
        //            Debug.Log("Score Updating Failed");

        //    }


        //    public void Login()
        //    {
        //        Social.localUser.Authenticate(OnAuthenticationComplete);
        //    }

        //    public void Logout()
        //    {
        //        PlayGamesPlatform.Instance.SignOut();
        //    }

        //    public bool IsSignedIn()
        //    {
        //        return PlayGamesPlatform.Instance.localUser.authenticated;
        //    }

        //    public void AddToLeaderBoard(long score, string board, Action OnSuccess, Action OnError)
        //    {
        //        if (IsSignedIn())
        //        {
        //            PlayGamesPlatform.Instance.ReportScore(score, board, (bool success) =>
        //            {
        //                if (success)
        //                    OnSuccess();
        //                else
        //                    OnError();
        //            });
        //        }
        //    }

        //    public void AddToLeaderBoard(long score, string board, string tag, Action OnSuccess, Action OnError)
        //    {
        //        if (IsSignedIn())
        //        {
        //            PlayGamesPlatform.Instance.ReportScore(score, board, tag, (bool success) =>
        //            {
        //                if (success)
        //                    OnSuccess();
        //                else
        //                    OnError();
        //            });
        //        }
        //    }

        //    public void ShowDefaultLeaderBoards(string LeaderboardId = null)
        //    {
        //        if (string.IsNullOrEmpty(LeaderboardId))
        //            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        //        else
        //            PlayGamesPlatform.Instance.ShowLeaderboardUI(LeaderboardId);
        //    }

        //    public void GetLeaderBoardsAroundPlayer(string leaderboardId, int rowcount, Action<LeaderboardScoreData> OnFetched,
        //                                             LeaderboardCollection collection = LeaderboardCollection.Public,
        //                                             LeaderboardTimeSpan timeSpan = LeaderboardTimeSpan.AllTime)
        //    {
        //        if (IsSignedIn())
        //        {
        //            PlayGamesPlatform.Instance.LoadScores(leaderboardId, LeaderboardStart.PlayerCentered,
        //            rowcount, collection, timeSpan,
        //            (data) =>
        //            {
        //                OnFetched(data);
        //            });
        //        }
        //    }

        //    public void GetTopLeaderBoards(string leaderboardId, int rowcount, Action<LeaderboardScoreData> OnFetched,
        //                                             LeaderboardCollection collection = LeaderboardCollection.Public,
        //                                             LeaderboardTimeSpan timeSpan = LeaderboardTimeSpan.AllTime)
        //    {
        //        if (IsSignedIn())
        //        {
        //            PlayGamesPlatform.Instance.LoadScores(leaderboardId, LeaderboardStart.TopScores,
        //             rowcount, collection, timeSpan,
        //            (data) =>
        //            {
        //                OnFetched(data);
        //            });
        //        }
        //    }
        //}

        //public class PlayerData
        //{
        //    public string name;
        //    public PlayerData()
        //    {

        //    }
        //    public PlayerData(string name)
        //    {
        //        this.name = name;
        //    }
        //}
    }
}