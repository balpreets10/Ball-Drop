using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class MenuContainer : PrimaryMenu
    {
        public ScrollRect MenuScrollRect;
        [SerializeField]
        private CanvasGroup mCanvasGroup;

        [SerializeField]
        private List<ScrollItem> MenuItems = new List<ScrollItem>();

        private void OnEnable()
        {
            MyEventManager.Instance.ScrollToMenu.AddListener(GoToMenu);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.ScrollToMenu.RemoveListener(GoToMenu);
            }
        }

        public override void Start()
        {
            base.Start();
            GoToMenu(1);
            MenuItems[1].ShowItem();
        }

        public void GoToMenu(int index)
        {
            if (index >= MenuItems.Count)
            {
                Debug.LogError("Provided Index is out of bounds of Menu Items\nPlease check index being passed");
                return;
            }
            if (index < 0)
            {
                Debug.LogError("Menu Index cant be less than 0");
                return;
            }
            if (index == 2 || index == 0)
            {
                UnityAdManager.Instance.ShowBanner(UnityEngine.Advertisements.BannerPosition.TOP_CENTER);
            }
            else
            {
                UnityAdManager.Instance.HideBanner();
            }

            float scrollValue = index / (float)(MenuItems.Count - 1);
            LeanTween.value(MenuScrollRect.horizontalScrollbar.value, scrollValue, 0.2f).setEase(LeanTweenType.easeInSine).setOnUpdate(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            MenuScrollRect.horizontalScrollbar.value = value;
        }

        [ContextMenu("Get Menu Items")]
        public void GetMenuItems()
        {
            for (int i = 0; i < MenuScrollRect.content.childCount; i++)
            {
                MenuItems.Add(MenuScrollRect.content.GetChild(i).GetComponent<ScrollItem>());
            }
        }

    }
}