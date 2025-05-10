using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public int Granularity = 5; // how many frames to wait until you re-calculate the FPS
    private List<double> times;
    private int counter = 5;
    public TMPro.TextMeshProUGUI FPSView;

    public void Start()
    {
        times = new List<double>();
    }

    public void Update()
    {
        if (counter <= 0)
        {
            CalcFPS();
            counter = Granularity;
        }

        times.Add(Time.deltaTime);
        counter--;
    }

    public void CalcFPS()
    {
        double sum = 0;
        foreach (double F in times)
        {
            sum += F;
        }

        double average = sum / times.Count;
        double fps = 1.00f / average;
        FPSView.text = "FPS = " + fps.ToString("F3");
    }
}