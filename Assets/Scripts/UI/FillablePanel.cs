using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class FillablePanel : MonoBehaviour
    {
        public Fillable SlowDown, Shield, X, Reverse;
        public RectTransform EffectTextRectTransform;
        public Transform GameMenu;
        private int Counter = 0;

        private void Start()
        {
            SlowDown.SetFillablePanel(this);
            Shield.SetFillablePanel(this);
            X.SetFillablePanel(this);
            Reverse.SetFillablePanel(this);
            LeanTween.scaleY(gameObject, 0, 0.1f);
        }

        private void OnEnable()
        {
            MyEventManager.Game.OnPlayerDeath.AddListener(FinishUp);
            MyEventManager.Game.OnLandedOnXCube.AddListener(OnLandedOnX);
            MyEventManager.Game.OnLandedOnReverseCube.AddListener(OnLandedOnReverseCube);
            MyEventManager.Game.Powerups.OnShieldCollected.AddListener(OnShieldCollected);
            MyEventManager.Game.Powerups.OnSlowDownCollected.AddListener(OnSlowDownCollected);
        }

        private void OnDisable()
        {
            MyEventManager.Game.OnPlayerDeath.RemoveListener(FinishUp);
            MyEventManager.Game.OnLandedOnXCube.RemoveListener(OnLandedOnX);
            MyEventManager.Game.OnLandedOnReverseCube.RemoveListener(OnLandedOnReverseCube);
            MyEventManager.Game.Powerups.OnShieldCollected.RemoveListener(OnShieldCollected);
            MyEventManager.Game.Powerups.OnSlowDownCollected.RemoveListener(OnSlowDownCollected);
        }

        private void FinishUp()
        {
            LeanTween.scaleY(gameObject, 0, 0.2f);
        }

        private void OnSlowDownCollected(float duration)
        {
            OnFillableActivated();
            SlowDown.Activate(duration);
            ShowEffectText("SlowDown");
        }

        private void OnShieldCollected(float duration)
        {
            OnFillableActivated();
            Shield.Activate(duration);
            ShowEffectText("Shield");
        }

        private void OnLandedOnReverseCube(float duration)
        {
            OnFillableActivated();
            Reverse.Activate(duration);
            ShowEffectText("Reverse");
        }

        private void OnLandedOnX(float duration, float scalefactor)
        {
            OnFillableActivated();
            X.Activate(duration);
            ShowEffectText("Scale Up");
        }

        public void OnFillableActivated()
        {
            Counter++;
            if (Counter == 1)
                LeanTween.scaleY(gameObject, 1, 0.2f);
        }

        public void OnFillableDeactivated()
        {
            Counter--;
            if (Counter == 0)
                LeanTween.scaleY(gameObject, 0, 0.2f);
        }

        private void ShowEffectText(string Message)
        {
            ObjectPool.Instance.GetEffectText().Activate(GameMenu, EffectTextRectTransform.anchoredPosition3D, Message);
        }
    }
}