using BallDrop.Manager;
using Facebook.Unity;
//using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BallDrop
{
    public class MainMenu : ScrollItem
    {
        public GameObject FacebookPanel;
        public GameObject GooglePanel;
        public GameObject Name;
        public GameObject PicturePanel;
        public GameObject ProfilePanel;
        public GameObject CurrentLevelBackground;
        public GameObject Ball;
        public GameObject Coins;
        public GameObject CoinEffect;
        public GameObject CoinEffectDest;
        public TMP_InputField tempLevel;
        public TextMeshProUGUI Username;
        public TextMeshProUGUI TxtHighScoreClassic;
        public TextMeshProUGUI TxtHighScoreArcade;
        public TextMeshProUGUI LevelText;
        public Button AdButton;
        public Image ProfilePicture, TitleMask;
        public List<GameObject> Buttons;
        //Tweens
        private LTDescr currentLevelRotation;
        private Vector3 anchoredPos;

        private void OnEnable()
        {
            MyEventManager.OnGameRewardAdReady.AddListener(OnGameRewardAdReady);
            MyEventManager.OnCompletedAwardAd.AddListener(OnCompletedAwardAd);
            MyEventManager.OnFacebookLoginComplete.AddListener(OnFacebookLoginComplete);
            MyEventManager.UpdateUI.AddListener(UpdateUI);
        }

        private void OnDisable()
        {
                MyEventManager.OnGameRewardAdReady.RemoveListener(OnGameRewardAdReady);
                MyEventManager.OnCompletedAwardAd.RemoveListener(OnCompletedAwardAd);
                MyEventManager.OnFacebookLoginComplete.RemoveListener(OnFacebookLoginComplete);
                MyEventManager.UpdateUI.RemoveListener(UpdateUI);
        }

        private void Start()
        {
            if (PreferenceManager.Instance.GetBoolPref(PrefKey.FacebookLogin))
                FacebookPanel.SetActive(false);
            string pic = PreferenceManager.Instance.GetStringPref(PrefKey.ProfilePic, null);
            if (!string.IsNullOrEmpty(pic))
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(Convert.FromBase64String(pic));
                ProfilePicture.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            }
            AdButton.SetActive(UnityAdManager.Instance.CheckGameRewardReady());
            anchoredPos = CoinEffect.GetComponent<RectTransform>().anchoredPosition3D;
        }

        public void StartClassic()
        {
            StartGame(GameMode.Classic);
        }

        public void StartArcade()
        {
            StartGame(GameMode.Arcade);
        }

        public void StartGame(GameMode mode)
        {
            IncrementGamesPlayed();
            MyEventManager.SetGameMode.Dispatch(mode);
        }

        private void UpdateUI()
        {
            UpdateTexts();
            AnimateHighScores();
        }

        private void UpdateTexts()
        {
            Username.text = PlayerDataManager.Instance.GetPlayerName();
            LevelText.text = PlayerDataManager.Instance.GetPlayerLevel().ToString();
            Coins.GetComponentInChildren<TextMeshProUGUI>().text = PlayerDataManager.Instance.GetCoins().ToString();
        }

        public override void ShowItem()
        {
            base.ShowItem();
            DisableButtons();
            TxtHighScoreClassic.text = TxtHighScoreArcade.text = GameStrings.EmptyString;
            ProfilePanel.transform.localScale = new Vector3(0, 1, 1);
            CurrentLevelBackground.transform.localScale = new Vector3(1, 0, 1);
            foreach (GameObject btn in Buttons)
            {
                btn.transform.localScale = Vector3.zero;
            }
            if (GameData.Instance.buildType == BuildType.Test)
            {
                tempLevel.gameObject.SetActive(true);
            }
            else
            {
                tempLevel.gameObject.SetActive(false);
            }

            LeanTween.scaleX(ProfilePanel, 1, .5f).setOnComplete(ScaleProfilePicture);
            Ball.SetActive(true);
            LeanTween.value(-700, 600, 1f).setOnUpdate(MoveBall).setOnComplete(DeactivateBall);
            LeanTween.value(0f, 1f, 1.5f).setOnUpdate(UpdateFill);
            UpdateTexts();
            CheckForDialogs();
        }

        private void CheckForDialogs()
        {
            if (MySceneManager.Instance.GetPrevious() == Scenes.Splash)
            {
                CheckConnection.Instance.CheckInternet(() =>
                {
                    StartCoroutine(WaitForLogin());
                }, () =>
                {
                    EnableButtons();
                });
            }
            else
            {
                EnableButtons();
            }
        }

        private void ScaleProfilePicture()
        {
            if (ProfilePicture.sprite != null)
            {
                ShowProfilePicture();
            }
            ScaleLevelPanel();
        }

        private void ShowProfilePicture()
        {
            LeanTween.scale(PicturePanel, Vector3.one, .8f);
        }

        private void ScaleLevelPanel()
        {
            LeanTween.scaleY(CurrentLevelBackground, 1, .5f).setOnComplete(AnimateButtons);
        }

        private void AnimateButtons()
        {
            foreach (GameObject btn in Buttons)
            {
                LeanTween.scale(btn, Vector3.one, .5f);
            }
            InitLevelPanel();
        }

        private void InitLevelPanel()
        {
            LeanTween.rotateAroundLocal(CurrentLevelBackground, Vector3.forward, 40f, .75f).setEase(LeanTweenType.easeOutSine).setOnComplete(AnimateLevelPanel);
        }
        private void AnimateLevelPanel()
        {
            currentLevelRotation = LeanTween.rotateAroundLocal(CurrentLevelBackground, Vector3.forward, -80f, 1.5f).setLoopPingPong().setEase(LeanTweenType.easeInOutQuad);
            AnimateHighScores();
        }

        private void AnimateHighScores()
        {
            int HighScoreArcade = PlayerDataManager.Instance.GetArcadeScore();
            int CurrentScoreClassic = PlayerDataManager.Instance.GetClassicScore();

            if (HighScoreArcade != 0)
            {
                LeanTween.value(0, HighScoreArcade, .5f).setOnUpdate(UpdateArcadeScore);
            }
            if (CurrentScoreClassic != 0)
            {
                LeanTween.value(0, CurrentScoreClassic, .5f).setOnUpdate(UpdateClassicScore);
            }
        }

        private void UpdateArcadeScore(float score)
        {
            TxtHighScoreArcade.text = (int)score + GameStrings.EmptyString;
        }

        private void UpdateClassicScore(float score)
        {
            TxtHighScoreClassic.text = (int)score + GameStrings.EmptyString;
        }

        private void MoveBall(float val)
        {
            Ball.GetComponent<RectTransform>().anchoredPosition = new Vector2(val, Ball.GetComponent<RectTransform>().anchoredPosition.y);
        }

        private void UpdateFill(float val)
        {
            TitleMask.fillAmount = val;
        }

        private void DeactivateBall()
        {
            Ball.SetActive(false);
        }

        private void OnGameRewardAdReady()
        {
            AdButton.SetActive(UnityAdManager.Instance.CheckGameRewardReady());
        }

        public void ShowAd()
        {
            UnityAdManager.Instance.ShowGameRewardVideo();
        }

        private void OnCompletedAwardAd(int amount)
        {
            AdButton.SetActive(UnityAdManager.Instance.CheckGameRewardReady());
            CoinEffect.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount;
            LeanTween.scale(CoinEffect, new Vector3(1.3f, 1.3f, 1), .4f).setOnComplete(ScaleDown, amount);
        }

        private void ScaleDown(object amount)
        {
            LeanTween.scale(CoinEffect, new Vector3(.8f, .8f, 1), .2f).setOnComplete(MoveToCoins, amount);
        }

        private void MoveToCoins(object amount)
        {
            LeanTween.move(CoinEffect, CoinEffectDest.transform, .5f).setOnComplete(UpdateText, amount);
        }

        private void UpdateText(object amount)
        {
            CoinEffect.transform.localScale = Vector3.zero;
            CoinEffect.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            int c = int.Parse(Coins.GetComponentInChildren<TextMeshProUGUI>().text.ToString());
            LeanTween.value(c, c + (int)amount, 1f).setOnUpdate(UpdateCoinsText);
        }

        private void UpdateCoinsText(float value)
        {
            Coins.GetComponentInChildren<TextMeshProUGUI>().text = ((int)value).ToString();
        }

        private void IncrementGamesPlayed()
        {
            PreferenceManager.Instance.IncrementIntPref(PrefKey.GamesPlayed, 0);
        }

        public void SetLevel()
        {
            try
            {
                int level = int.Parse(tempLevel.text);
                if (level > 0)
                {
                    PlayerDataManager.Instance.SetLevel(level);
                    LevelText.text = level.ToString();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void OnFacebookLoginComplete(FBData fBData)
        {
            Debug.unityLogger.Log(GameData.TAG, "Fb login complete, updating main menu");
            FacebookPanel.SetActive(false);
            Name.transform.localScale = new Vector3(0, 1, 1);
            Username.text = "<b>" + fBData.name + "</b>";
            StartCoroutine(LoadProfilePicture(ProfilePicture, fBData.picture.data.url));
            LeanTween.scaleX(Name, 1, 0.8f);
        }

        public void LoginWithGoogle()
        {
            MyEventManager.LoginWithGoogle.Dispatch();
        }

        private void ScoreCallBack(IGraphResult result)
        {
            if (result.Error != null)
            {
                Debug.LogError(result.Error);
                return;
            }
            foreach (object obj in result.ResultList)
            {
                Debug.Log(obj);
            }
        }

        private void DisableButtons()
        {
            foreach (GameObject btn in Buttons)
            {
                btn.GetComponent<Button>().interactable = false;
            }
        }

        private void EnableButtons()
        {
            Debug.unityLogger.Log(GameData.TAG, "Enable buttons");
            foreach (GameObject btn in Buttons)
            {
                btn.GetComponent<Button>().interactable = true;
            }
        }

        private IEnumerator WaitForLogin()
        {
            while (!PlayfabManager.Instance.IsLoggedIn())
            {
                yield return null;
            }
            if (PlayfabManager.Instance.IsNewlyCreated())
            {
                MyEventManager.GetPlayerName.Dispatch();
            }
            else if (!PlayfabManager.Instance.IsNewlyCreated() && !PreferenceManager.Instance.GetBoolPref(PrefKey.NotFirstGameOnline))
            {
                MyEventManager.ShowMessage.Dispatch("Welcome back " + PlayerDataManager.Instance.GetPlayerName() + GameStrings.AccountRestorationMsg);
            }
            EnableButtons();

        }

        public IEnumerator LoadProfilePicture(Image ProfilePicture, string uri)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                ProfilePicture.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                PreferenceManager.Instance.UpdateStringPref(PrefKey.ProfilePic, Convert.ToBase64String(tex.EncodeToPNG()));
                ShowProfilePicture();
            }
        }
    }
}