using BallDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FbPopup : Popup
{
    public Image Background;

    private void Start()
    {
        base.HidePopup();
    }

    public void Login()
    {
        CheckConnection.Instance.CheckInternet(OnAvailable, OnUnavailable);
        MySceneManager.Instance.ShowLoadingCanvas();
        HidePopup();
    }

    private void OnAvailable()
    {
        MyEventManager.Menu.LoginWithFacebook.Dispatch();
    }

    private void OnUnavailable()
    {
        MySceneManager.Instance.HideLoadingCanvas();
        MyEventManager.ShowMessage.Dispatch(GameStrings.InternetUnavailableMsg);
    }
}