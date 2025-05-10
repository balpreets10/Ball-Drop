using System;
using System.Collections;
using BallDrop.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class GameMenu : PrimaryMenu
    {
        public Canvas ParentCanvas;
        public GraphicRaycaster graphicRaycaster;

        private Image CoinsImage;
        public PiggyController PiggyBank;
        public TextMeshProUGUI ScoreText, CoinsText;
        public RectTransform ScoreRect;
        public RectTransform EffectTextRectTransform;

        [Header("Arcade Objects")]
        public GameObject ArcadePanel;
        public TextMeshProUGUI ScoreTextArcade, CoinsTextArcade;
        public Image CoinsImageArcade;

        [Header("Classic Objects")]
        public GameObject ClassicPanel;
        public TextMeshProUGUI ScoreTextClassic, CoinsTextClassic, RowCountClassic, RowsLeftCLassic, LevelText, NextLevelText;
        public RectTransform Star1_Transform, Star2_Transform, Star3_Transform;
        public Image ScoreFill, CoinImageClassic;
        private bool Star1Animated, Star2Animated, Star3Animated;

        [Header("Progress Objects Classic")]
        public Slider ProgressSlider;
        public GameObject ScoreSlider;

        [Header("Score Panel Properties")]
        private int UpdatedScore = 0;

        private int score = 0;

        private void OnEnable()
        {
            MyEventManager.OnScoreUpdated.AddListener(OnScoreChanged);
            MyEventManager.OnRowPassed.AddListener(OnRowPassed);
            MyEventManager.OnCoinsUpdated.AddListener(OnCoinsUpdated);
            ProgressSlider.wholeNumbers = false;
        }

        private void OnDisable()
        {
                MyEventManager.OnScoreUpdated.RemoveListener(OnScoreChanged);
                MyEventManager.OnRowPassed.RemoveListener(OnRowPassed);
                MyEventManager.OnCoinsUpdated.RemoveListener(OnCoinsUpdated);
        }

        public override void Start()
        {
            base.Start();
            if (GameData.Instance.gameMode == GameMode.Classic)
            {
                SetUIForClassic();
            }
            else
            {
                SetUIForArcade();
            }
            SetCoins();
        }

        private void OnScoreChanged(int Increment)
        {
            int temp = score;
            score += Increment;
            IDynamicScore dynamicScore = ObjectPool.Instance.GetDynamicScore().GetComponent<IDynamicScore>();
            if (dynamicScore != null)
                dynamicScore.ActivateAndStartAnimation(GameData.Instance.rowPassPosition, Increment, ScoreRect);
            else
                Debug.LogWarning("Dynamic Score is Null");

            LeanTween.value(gameObject, temp, score, .3f)
                .setOnUpdate(ShowScoreChanges).setEase(LeanTweenType.linear).setOnComplete(OnScoreUpdatedOnSlider);
        }

        private void OnRowPassed()
        {
            RowCountClassic.text = GameData.Instance.playerGameData.RowsPassed.ToString();
            if (GameData.Instance.gameMode == GameMode.Classic)
            {
                LeanTween.value(ProgressSlider.gameObject, OnValueChanged, ProgressSlider.value, GameData.Instance.playerGameData.RowsPassed, .1f);
            }
        }

        private void OnCoinsUpdated(int coins)
        {
            PiggyBank.Animate();
            StartCoroutine(UpdateCoinsText());
        }

        private IEnumerator UpdateCoinsText()
        {
            yield return new WaitForSeconds(1.5f);
            LeanTween.scale(CoinsImage.gameObject, new Vector3(1.5f, 1.5f, 1), .5f).setLoopPingPong(2);
            if (CoinsText != null)
                CoinsText.text = PlayerDataManager.Instance.GetCoins() + GameStrings.EmptyString;
            else
                Debug.LogWarning("Assign Coin Text Variable First");
        }

        private void SetUIForClassic()
        {
            ClassicPanel.SetActive(true);
            ArcadePanel.SetActive(false);
            ScoreText = ScoreTextClassic;
            ScoreRect = ScoreText.GetComponent<RectTransform>();
            CoinsText = CoinsTextClassic;
            CoinsImage = CoinImageClassic;
            ProgressSlider.maxValue = GameData.Instance.levelData.RowCount;
            ScoreFill.fillAmount = 0;
            RowsLeftCLassic.text = "/" + GameData.Instance.levelData.RowCount;
            RowCountClassic.text = "0";
            NextLevelText.text = (GameData.Instance.levelData.Level + 1).ToString();
            AnimateLevelHolder();
        }

        private void SetUIForArcade()
        {
            ClassicPanel.SetActive(false);
            ArcadePanel.SetActive(true);
            ScoreText = ScoreTextArcade;
            ScoreRect = ScoreText.GetComponent<RectTransform>();
            CoinsText = CoinsTextArcade;
            CoinsImage = CoinsImageArcade;

            Invoke("AnimateScore", .9f);
        }

        private void SetCoins()
        {
            CoinsText.text = PlayerDataManager.Instance.GetCoins().ToString();
        }

        private void AnimateLevelHolder()
        {
            LevelText.text = GameData.Instance.levelData.Level.ToString();
            AnimateStars();
        }

        private void AnimateScore()
        {
            LeanTween.value(0f, 1f, .5f).setOnUpdate(UpdateScale).setEaseOutBounce();
        }

        private void UpdateScale(float val)
        {
            ScoreText.transform.parent.localScale = new Vector3(val, val, 1f);
        }

        private void AnimateStars()
        {
            LeanTween.value(gameObject, OnUpdateStars, 1, 1.5f, 1f).setLoopPingPong(1).setOnComplete(OnAnimationComplete);
        }

        private void OnUpdateStars(float scale)
        {
            Star1_Transform.localScale = Star2_Transform.localScale = Star3_Transform.localScale = new Vector3(1, scale, 1);
        }

        private void OnAnimationComplete()
        {
            Star1_Transform.localScale = Star2_Transform.localScale = Star3_Transform.localScale = Vector3.one;
            Star1_Transform.localRotation = Star2_Transform.localRotation = Star3_Transform.localRotation = Quaternion.identity;

        }



        private void OnValueChanged(float value)
        {
            ProgressSlider.value = value;
        }

        private void ShowScoreChanges(float val)
        {
            if (ScoreText != null)
                ScoreText.text = (int)val + GameStrings.EmptyString;
            else
                Debug.LogWarning("Score Text is Null");

            if (GameData.Instance.gameMode == GameMode.Classic)
                ScoreFill.fillAmount = val / GameData.Instance.levelData.GoldScore;
            UpdatedScore = Mathf.FloorToInt(val);
        }

        private void OnScoreUpdatedOnSlider()
        {
            if (GameData.Instance.gameMode == GameMode.Classic)
            {
                if (UpdatedScore > 0)
                {
                    if (UpdatedScore >= GameData.Instance.levelData.GoldScore && !Star3Animated)
                    {
                        LeanTween.scale(Star3_Transform, Vector2.one * 1.2f, .8f).setEase(LeanTweenType.easeShake).setOnComplete(RotateStar, Star3_Transform.gameObject);
                        Star3Animated = true;
                    }
                    else if (UpdatedScore >= GameData.Instance.levelData.SilverScore && !Star2Animated)
                    {
                        LeanTween.scale(Star2_Transform, Vector2.one * 1.2f, .8f).setEase(LeanTweenType.easeShake).setOnComplete(RotateStar, Star2_Transform.gameObject);
                        Star2Animated = true;
                    }
                    else if (UpdatedScore >= GameData.Instance.levelData.BronzeScore && !Star1Animated)
                    {
                        LeanTween.scale(Star1_Transform, Vector2.one * 1.2f, .8f).setEase(LeanTweenType.easeShake).setOnComplete(RotateStar, Star1_Transform.gameObject);
                        Star1Animated = true;
                    }
                }
            }
        }

        private void RotateStar(object gameobject)
        {
            LeanTween.rotateAroundLocal((GameObject)gameobject, Vector3.up, 360, 1f).setLoopType(LeanTweenType.linear);
        }

        public void PauseGame()
        {
            MyEventManager.PauseGame.Dispatch();
        }

    }
}