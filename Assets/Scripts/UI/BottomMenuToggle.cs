using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class BottomMenuToggle : UIComponent
    {
        private Toggle myToggle;
        public GameObject Background;
        public GameObject Icon;
        public GameObject Text;
        private Vector3 IsOnScale = Vector3.one * 1.15f;
        private Vector3 movePosition = new Vector3(0, 50, 0);

        protected override void Start()
        {
            base.Start();
            myToggle = GetComponent<Toggle>();
            myToggle.isOn = false;
            Background.transform.localScale = Vector3.zero;
            Icon.transform.localScale = Vector3.one;
            Icon.transform.localPosition = Vector3.zero;
            Text.transform.localScale = Vector3.zero;
            //myToggle.onValueChanged.AddListener(OnValueChanged);
        }

        public void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                LeanTween.scale(Background, Vector3.one, .15f).setEase(LeanTweenType.easeOutBack);
                LeanTween.scale(Icon, IsOnScale, .15f).setEase(LeanTweenType.linear);
                LeanTween.moveLocal(Icon, movePosition, .15f).setEase(LeanTweenType.linear);
                LeanTween.scale(Text, Vector3.one, .15f).setEase(LeanTweenType.linear);
                MyEventManager.Menu.ScrollToMenu.Dispatch(transform.GetSiblingIndex());
            }
            else
            {
                LeanTween.scale(Background, Vector3.zero, .1f).setEase(LeanTweenType.easeInBack);
                LeanTween.scale(Icon, Vector3.one, .15f).setEase(LeanTweenType.linear);
                LeanTween.moveLocal(Icon, Vector3.zero, .15f).setEase(LeanTweenType.linear);
                LeanTween.scale(Text, Vector3.zero, .15f).setEase(LeanTweenType.linear);
            }
        }
    }
}