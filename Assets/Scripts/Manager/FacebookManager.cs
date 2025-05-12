using BallDrop.Manager;
using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class FacebookManager : SingletonMonoBehaviour<FacebookManager>
    {
        private void Start()
        {
            InitializeFacebook();
        }

        private void OnEnable()
        {
            MyEventManager.Menu.LoginWithFacebook.AddListener(Login);
        }

        private void OnDisable()
        {
            MyEventManager.Menu.LoginWithFacebook.RemoveListener(Login);
        }

        public void InitializeFacebook()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(OnInitComplete, OnHideUnity);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        private void OnInitComplete()
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Access Token = " + AccessToken.CurrentAccessToken.TokenString);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            //Debug.Log(string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown));
            //Debug.Log("Is game shown: " + isGameShown);
        }

        public void Login()
        {
            StartCoroutine(WaitForInitialization());
            //Debug.Log("Logging in to facebook");
        }

        private IEnumerator WaitForInitialization()
        {
            if (!FB.IsInitialized)
            {
                while (!FB.IsInitialized)
                    yield return null;
            }
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, OnFacebookLoggedIn);
        }

        private void OnFacebookLoggedIn(ILoginResult result)
        {
            PreferenceManager.Instance.UpdateBoolpref(PrefKey.NotFirstGameOnline, true);
            if (result == null || string.IsNullOrEmpty(result.Error))
            {
                Debug.unityLogger.Log(GameData.TAG, "Facebook Auth Complete!");
                FB.API("/me?fields=name,email,picture.height(256)", HttpMethod.GET, APICallback);
            }
            else
            {
                Debug.unityLogger.Log(GameData.TAG, "Facebook Auth Failed: " + result.Error + " --- " + result.RawResult);
                MyEventManager.OnUserDataFetched.Dispatch(null);
            }
        }

        private void APICallback(IGraphResult result)
        {
            Debug.unityLogger.Log(GameData.TAG, result.RawResult);
            FBData fBData = JsonUtility.FromJson<FBData>(result.RawResult.ToString());
            fBData.accessToken = AccessToken.CurrentAccessToken;
            PreferenceManager.Instance.UpdateStringPref(PrefKey.PlayerName, fBData.name);
            MyEventManager.Menu.OnFacebookLoginComplete.Dispatch(fBData);
        }

        //public void GetFriendList()
        //{
        //    //ClickBlocker.SetActive(true);
        //    if (AccessToken.CurrentAccessToken != null)
        //    {
        //        FbData.Instance.accessToken = AccessToken.CurrentAccessToken;
        //        FB.API("/me/friends", HttpMethod.GET, OnFriendListFetched);
        //    }
        //}
    }

    [Serializable]
    public struct FBData
    {
        public string name;
        public string email;
        public string id;
        public AccessToken accessToken;
        public Picture picture;
    }

    [Serializable]
    public class Picture
    {
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public int height;
        public int width;
        public string url;
    }
}