using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Image LoadingBar;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(bool activate)
    {
        if (activate)
        {
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            StartLoadingClockwise();
        }
        else
        {
            LeanTween.cancel(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void StartLoadingClockwise()
    {
        LoadingBar.fillClockwise = true;
        LeanTween.value(gameObject, OnFillComplete, 0, 1, 1f).setOnComplete(StartLoadingCounterClockwise);
    }

    private void OnFillComplete(float fillAmount)
    {
        LoadingBar.fillAmount = fillAmount;
    }

    private void StartLoadingCounterClockwise()
    {
        LoadingBar.fillClockwise = false;
        LeanTween.value(gameObject, OnFillComplete, 1, 0, 1f).setOnComplete(StartLoadingClockwise);
    }
}