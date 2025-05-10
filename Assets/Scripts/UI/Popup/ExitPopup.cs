using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class ExitPopup : Popup
    {

        public override void HidePopup(bool instant = false)
        {
            base.HidePopup(instant);
            if (UnityAdManager.Instance.CheckInterstitialReady())
                UnityAdManager.Instance.ShowInterstitial();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}