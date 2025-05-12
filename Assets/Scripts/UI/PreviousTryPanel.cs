using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace BallDrop
{
    public class PreviousTryPanel : UIComponent
    {
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI BestText;
        public RectTransform mRectTransform;

        protected override void Start()
        {
            base.Start();
            mRectTransform = GetComponent<RectTransform>();
            //LeanTween.moveLocalX(gameObject, Screen.width + mRectTransform.sizeDelta.x, 0f);
        }

        private void OnEnable()
        {
            MyEventManager.Game.OnPlayerActivated.AddListener(OnPlayerActivated);
        }

        private void OnDisable()
        {
            MyEventManager.Game.OnPlayerActivated.RemoveListener(OnPlayerActivated);
        }

        private void OnPlayerActivated()
        {
            if (GameData.Instance.gameMode == GameMode.Arcade)
            {
                if (PreferenceManager.Instance.GetIntPref(PrefKey.PreviousScoreArcade, 0) != 0)
                {
                    Activate();
                }
            }
        }

        public override void Activate()
        {
            base.Activate();
            ScoreText.text = "0";
            BestText.text = "0";
            float pos = 480;
            LeanTween.moveLocalX(gameObject, pos, .5f).setOnComplete(ReturnBack).setDelay(1.5f);
        }

        private void ReturnBack()
        {
            LeanTween.value(0, PreferenceManager.Instance.GetIntPref(PrefKey.PreviousScoreArcade, 0), .3f).setOnUpdate(UpdateLastTryScore);
            LeanTween.value(0, PlayerDataManager.Instance.GetArcadeScore(), .6f).setOnUpdate(UpdateBestTryScore);
            LeanTween.moveLocalX(gameObject, Screen.width + mRectTransform.sizeDelta.x, .5f).setDelay(4f);
        }

        private void UpdateLastTryScore(float score)
        {
            ScoreText.text = (int)score + GameStrings.EmptyString;
        }

        private void UpdateBestTryScore(float score)
        {
            BestText.text = (int)score + GameStrings.EmptyString;
        }
    }
}