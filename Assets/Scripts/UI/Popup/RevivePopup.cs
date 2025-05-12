using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class RevivePopup : Popup
    {
        public Button PlayAd;

        private void OnEnable()
        {
            MyEventManager.ReviveOption.AddListener(ShowPopup);
            MyEventManager.OnCompletedRevivalAd.AddListener(HidePopup);
        }

        private void OnDisable()
        {
            MyEventManager.ReviveOption.RemoveListener(ShowPopup);
            MyEventManager.OnCompletedRevivalAd.RemoveListener(HidePopup);
        }

        public override void ShowPopup()
        {
            base.ShowPopup();
        }

        public override void OnPopupShown()
        {
            base.OnPopupShown();
            Time.timeScale = 0f;
        }

        public void WatchAd()
        {
            UnityAdManager.Instance.ShowLifeRewardVideo();
        }

        public void Cancel()
        {
            MyEventManager.OnCancelledRevive.Dispatch();
            HidePopup();
        }

        public void HidePopup()
        {
            Time.timeScale = 1f;
            base.HidePopup(true);
        }
    }
}