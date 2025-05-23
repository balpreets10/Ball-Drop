﻿using BallDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using BallDrop.Manager;

public class NamePopup : Popup
{
    [SerializeField]
    private TMP_InputField username;

    [SerializeField]
    private Button OkBtn;

    private void OnEnable()
    {
        MyEventManager.GetPlayerName.AddListener(ShowPopup);
    }

    private void OnDisable()
    {
        MyEventManager.GetPlayerName.RemoveListener(ShowPopup);
    }

    public void Start()
    {
        base.HidePopup();
        OkBtn.interactable = false;
        username.onValueChanged.AddListener(OnTextValueChanged);
    }

    public override void ShowPopup()
    {
        PreferenceManager.Instance.UpdateBoolpref(PrefKey.NotFirstGameOnline, true);
        if (!PlayerDataManager.Instance.nameSet)
        {
            base.ShowPopup();
        }
    }

    private void OnTextValueChanged(string name)
    {
        if (!string.IsNullOrEmpty(name))
            OkBtn.interactable = true;
        else
            OkBtn.interactable = false;
    }

    public void OnOKClick()
    {
        PreferenceManager.Instance.UpdateStringPref(PrefKey.PlayerName, username.text);
        PlayerDataManager.Instance.UpdatePlayerName(username.text);
        PlayerDataManager.Instance.nameSet = true;
        MyEventManager.UpdateUI.Dispatch();
        HidePopup();
    }
}