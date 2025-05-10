using BallDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePopup : Popup
{
    public TextMeshProUGUI MessageText;

    private void OnEnable()
    {
        MyEventManager.ShowMessage.AddListener(ShowMessage);

    }

    private void OnDisable()
    {
            MyEventManager.ShowMessage.RemoveListener(ShowMessage);
    }

    public void Start()
    {
        base.HidePopup();
    }

    private void ShowMessage(string msg)
    {
        MessageText.text = msg;
        ShowPopup();
    }

    public void OkClicked()
    {
        base.HidePopup();
    }
}