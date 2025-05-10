using BallDrop;
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
    TMP_InputField username;
    [SerializeField]
    Button OkBtn;

    private void OnEnable()
    {
        MyEventManager.Instance.GetPlayerName.AddListener(ShowPopup);
    }

    private void OnDisable()
    {
        if (MyEventManager.Instance != null)
        {
            MyEventManager.Instance.GetPlayerName.RemoveListener(ShowPopup);
        }
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
        MyEventManager.Instance.UpdateUI.Dispatch();
        HidePopup();
    }
}
