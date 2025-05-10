using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour
{
    private bool IsTweening = false;
    private Button mButton;
    
    private void Start()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(OnClickEffect);
    }

    public void OnClickEffect()
    {
        if (!IsTweening)
        {
            LeanTween.cancel(gameObject);
            IsTweening = true;
            transform.localScale = Vector3.one;
            LeanTween.scale(gameObject, Vector3.one * 0.9f, .05f).setLoopPingPong(1).setEase(LeanTweenType.easeOutQuad).setOnComplete(OnTweenComplete);
        }
    }

    private void OnTweenComplete()
    {
        IsTweening = false;
    }
}
