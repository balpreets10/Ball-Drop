using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoInternetConnection : MonoBehaviour
{
    public Image Icon;
    private Color color = new Color(255, 255, 255);

    private void Start()
    {
        ShowNoInternetConnectionPanel();
    }
    public void ShowNoInternetConnectionPanel()
    {
        gameObject.SetActive(true);
        LeanTween.value(Icon.gameObject, 1, .1f, .5f).setOnUpdate(UpdateAlpha).setLoopPingPong();
        LeanTween.scale(Icon.gameObject, Vector3.one * 0.7f, .5f).setLoopPingPong();
    }

    public void Deactivate()
    {
        LeanTween.cancel(Icon.gameObject);
        Icon.transform.localScale = Vector3.one;
        color.a = 1;
        Icon.color = color;
        gameObject.SetActive(false);
    }

    private void UpdateAlpha(float alpha)
    {
        color.a = alpha;
        Icon.color = color;
    }
}
