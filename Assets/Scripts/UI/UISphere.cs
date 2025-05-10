using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISphere : MonoBehaviour
{
    public float XDistance;
    public float YDistance;
    TrailRenderer lineRenderer;
    RectTransform rectTransform;


    private void Awake()
    {
        lineRenderer = GetComponent<TrailRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void Start()
    {
        StartTween();

    }


    void StartTween()
    {
        LeanTween.rotateAround(gameObject, Vector3.one, 360, .5f).setLoopType(LeanTweenType.linear);
        LeanTween.moveLocalY(gameObject, -Screen.height, 2.5f).setOnComplete(ResetSphere).setDelay(1f);
        lineRenderer.enabled = true;
    }
    private void ResetSphere()
    {
        lineRenderer.enabled = false;
        rectTransform.anchoredPosition3D = new Vector3(UnityEngine.Random.Range(-500, 500), 200, 0);
        StartTween();
    }
}
