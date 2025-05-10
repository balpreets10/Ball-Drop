using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class RevivePopup : Popup
    {
        public Button PlayAd;

        private void OnEnable()
        {
            MyEventManager.Instance.ReviveOption.AddListener(ShowPopup);
            MyEventManager.Instance.OnCompletedRevivalAd.AddListener(HidePopup);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.ReviveOption.RemoveListener(ShowPopup);
                MyEventManager.Instance.OnCompletedRevivalAd.RemoveListener(HidePopup);
            }
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
            MyEventManager.Instance.OnCancelledRevive.Dispatch();
            HidePopup();
        }

        public void HidePopup()
        {
            Time.timeScale = 1f;
            base.HidePopup(true);
        }
    }
}