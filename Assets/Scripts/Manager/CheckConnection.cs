using BallDrop;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CheckConnection : SingletonMonoBehaviour<CheckConnection>
{
    public void CheckInternet(Action OnAvailable, Action OnUnavailable)
    {
        StartCoroutine(CheckNet(OnAvailable, OnUnavailable));
    }

    private IEnumerator CheckNet(Action OnAvailable, Action OnUnavailable)
    {
        string checkNetApi = "www.google.com";

        using (UnityWebRequest www = UnityWebRequest.Get(checkNetApi))
        {
            www.timeout = 5;
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.unityLogger.Log(GameData.TAG, "Error While Sending: " + www.error);
                OnUnavailable();
                Debug.unityLogger.Log(GameData.TAG, "Unavailable");
            }
            else
            {
                OnAvailable();
            }
        }
    }
}
