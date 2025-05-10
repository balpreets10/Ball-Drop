using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BallDrop
{
    public class LevelClearMenu : PrimaryMenu
    {
        public FbPopup fbPopup;

        public TextMeshProUGUI TxtScoreValue;
        public TextMeshProUGUI PanelTitle;
        public Image Background;

        public Button ButtonRestart, ButtonNextLevel, ButtonMainMenu;
        public GameObject ps;
        public Color ClearColor;
        public Color LoseColor;
        public Color ArcadeColor;
        public LeanTweenType LeanTweenType;

        //public Image StarPanel;
        public Dictionary<int, List<Image>> StarImages = new Dictionary<int, List<Image>>();

        public List<Image> StarBronze;
        public List<Image> StarSilver;
        public List<Image> StarGold;

        public TextMeshProUGUI TxtRowsPassed;
        public TextMeshProUGUI TxtNormal;
        public TextMeshProUGUI TxtEnemy;
        public TextMeshProUGUI TxtFaded;
        public TextMeshProUGUI TxtMoving;
        public TextMeshProUGUI TxtX;
        public TextMeshProUGUI TxtSpikes;
        public TextMeshProUGUI TxtReverse;

        public TextMeshProUGUI TitleRowsPassed;
        public TextMeshProUGUI TitleNormal;
        public TextMeshProUGUI TitleEnemy;
        public TextMeshProUGUI TitleFaded;
        public TextMeshProUGUI TitleMoving;
        public TextMeshProUGUI TitleX;
        public TextMeshProUGUI TitleSpikes;
        public TextMeshProUGUI TitleReverse;


        private Dictionary<CubeType, int> CubeData;
        private LevelEndData levelEndData;
      
        public override void Start()
        {
            base.Start();
            StartCoroutine(ResetMenu());
            levelEndData = GameData.Instance.levelEndData;
            CubeData = levelEndData.CubeData;
            if (levelEndData.LevelCleared)
            {
                PanelTitle.text = GameStrings.LevelCleared;
                TxtScoreValue.color = ClearColor;
                if (!StarImages.ContainsKey(1)) StarImages.Add(1, StarBronze);
                if (!StarImages.ContainsKey(2)) StarImages.Add(2, StarSilver);
                if (!StarImages.ContainsKey(3)) StarImages.Add(3, StarGold);
            }
            else
            {
                PanelTitle.text = GameStrings.GameOver;
                TxtScoreValue.color = LoseColor;
            }
            if (GameData.Instance.gameMode == GameMode.Arcade)
            {
                TxtScoreValue.color = ArcadeColor;
            }

            Background.SetActive(true);
            Background.fillAmount = 0;

            LeanTween.value(0, 1, 1.5f).setOnUpdate(UpdateBackgroundFillAmount);
            AnimateRowsPassed();
        }

        public void PlayNext()
        {
            PreferenceManager.Instance.IncrementIntPref(PrefKey.GamesPlayed, 0);
            StopParticleSystem();
            MyEventManager.Instance.SetGameMode.Dispatch(GameData.Instance.gameMode);
        }

        public void MainMenu()
        {
            StopParticleSystem();
            MySceneManager.Instance.LoadScene(Scenes.Menu);
        }

        private IEnumerator ResetMenu()
        {
            TxtScoreValue.transform.localScale = Vector3.one;
            ButtonNextLevel.transform.localScale = Vector3.zero;
            ButtonRestart.transform.localScale = Vector3.zero;
            ButtonMainMenu.transform.localScale = Vector3.zero;
            InitImages(StarBronze);
            InitImages(StarSilver);
            InitImages(StarGold);
            yield return null;
        }

        private void InitImages(List<Image> stars)
        {
            foreach (Image i in stars)
            {
                i.transform.localScale = Vector3.zero;
                i.SetActive(false);
            }
        }

        private void UpdateBackgroundFillAmount(float value)
        {
            Background.fillAmount = value;
        }

        private void AnimateRowsPassed()
        {
            LeanTween.moveLocalX(TitleRowsPassed.gameObject, 0, .15f).setOnComplete(UpdateRowsPassed);
        }

        private void UpdateRowsPassed()
        {
            if (GameData.Instance.gameMode == GameMode.Classic)
                LeanTween.value(0, levelEndData.RowsPassed, .15f).setOnUpdate(UpdateRowsPassedClassic).setOnComplete(AnimateNormalValues);
            else
                LeanTween.value(0, levelEndData.RowsPassed, .15f).setOnUpdate(UpdateRowsPassedArcade).setOnComplete(AnimateNormalValues);
        }

        private void UpdateRowsPassedClassic(float value)
        {
            TxtRowsPassed.text = (int)value + GameStrings.ForwardSlash + levelEndData.RowCount;
        }

        private void UpdateRowsPassedArcade(float value)
        {
            TxtRowsPassed.text = (int)value + GameStrings.EmptyString;
        }

        private void AnimateNormalValues()
        {
            LeanTween.moveLocalX(TitleNormal.gameObject, 0, .15f).setOnComplete(UpdateNormalCube);
        }

        private void UpdateNormalCube()
        {
            int count = CubeData[CubeType.Normal];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtNormal).setOnComplete(AnimateEnemyValues);
            else
            {
                TxtNormal.text = GameStrings.Zero;
                AnimateEnemyValues();
            }
        }

        private void AnimateEnemyValues()
        {
            LeanTween.moveLocalX(TitleEnemy.gameObject, 0, .15f).setOnComplete(UpdateEnemyCube);
        }

        private void AnimateFadedValues()
        {
            LeanTween.moveLocalX(TitleFaded.gameObject, 0, .15f).setOnComplete(UpdateFadedCube);
        }

        private void AnimateMovingValues()
        {
            LeanTween.moveLocalX(TitleMoving.gameObject, 0, .15f).setOnComplete(UpdateMovingCube);
        }

        private void AnimateXValues()
        {
            LeanTween.moveLocalX(TitleX.gameObject, 0, .15f).setOnComplete(UpdateXCube);
        }

        private void AnimateSpikeValues()
        {
            LeanTween.moveLocalX(TitleSpikes.gameObject, 0, .15f).setOnComplete(UpdateSpikeCube);
        }

        private void AnimateReverseValues()
        {
            LeanTween.moveLocalX(TitleReverse.gameObject, 0, .15f).setOnComplete(UpdateReverseCube);
        }

        private void UpdateEnemyCube()
        {
            int count = CubeData[CubeType.Enemy];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtEnemy).setOnComplete(AnimateFadedValues);
            else
            {
                TxtEnemy.text = GameStrings.Zero;
                AnimateFadedValues();
            }
        }

        private void UpdateFadedCube()
        {
            int count = CubeData[CubeType.Faded];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtFaded).setOnComplete(AnimateMovingValues);
            else
            {
                TxtFaded.text = GameStrings.Zero;
                AnimateMovingValues();
            }
        }

        private void UpdateMovingCube()
        {
            int count = CubeData[CubeType.Moving];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtMoving).setOnComplete(AnimateXValues);
            else
            {
                TxtMoving.text = GameStrings.Zero;
                AnimateXValues();
            }
        }

        private void UpdateXCube()
        {
            int count = CubeData[CubeType.X];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtX).setOnComplete(AnimateSpikeValues);
            else
            {
                TxtX.text = GameStrings.Zero;
                AnimateSpikeValues();
            }
        }

        private void UpdateSpikeCube()
        {
            int count = CubeData[CubeType.Spike];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtSpikes).setOnComplete(AnimateReverseValues);
            else
            {
                TxtSpikes.text = GameStrings.Zero;
                AnimateReverseValues();
            }
        }

        private void UpdateReverseCube()
        {
            int count = CubeData[CubeType.Reverse];
            if (count > 0)
                LeanTween.value(0, count, 0.15f).setOnUpdate(ChangeValue, TxtReverse).setOnComplete(UpdateScore);
            else
            {
                TxtReverse.text = GameStrings.Zero;
                UpdateScore();
            }
        }

        private void ChangeValue(float value, object text)
        {
            ((TextMeshProUGUI)(text)).text = (int)value + GameStrings.EmptyString;
        }

        private void UpdateScore()
        {
            ObjectPool.Instance.DeactivateLeftCubes();
            if (levelEndData.Score > 0)
            {
                LeanTween.value(gameObject, 0, levelEndData.Score, .3f).setOnUpdate(OnScoreLoaded).setOnComplete(UpdateStars);
            }
            else
            {
                TxtScoreValue.text = GameStrings.Zero;
                UpdateStars();
            }
        }

        private void UpdateStars()
        {
            if (levelEndData.Score > 0)
            {
                LeanTween.scale(TxtScoreValue.gameObject, Vector3.one * 1.5f, .3f).setLoopPingPong();
            }
            if (levelEndData.LevelCleared)
            {
                int Stars = levelEndData.GetStarValue();
                List<Image> StarsToShow = new List<Image>();
                StarImages.TryGetValue(Stars, out StarsToShow);
                for (int i = 0; i < StarsToShow.Count; i++)
                {
                    StarsToShow[i].SetActive(true);
                    LeanTween.scale(StarsToShow[i].gameObject, Vector3.one, i / 5 + 0.15f).setEaseInQuad().setOnComplete(StartRotation, StarsToShow[i].gameObject);
                }
                ScaleUpButton(ButtonNextLevel.gameObject, 0f);
                ButtonNextLevel.SetActive(true);
            }
            else
            {
                ScaleUpButton(ButtonRestart.gameObject, 0f);
            }
            ScaleUpButton(ButtonMainMenu.gameObject, 0.15f);
        }

        private void StartCongratulatoryParticleSystem()
        {
            ps = ObjectPool.Instance.GetCongratulatoryParticleSystem();
            //Vector3 position = new Vector3(-5.1f, -3.21f, 0);
            ps.transform.SetParent(Camera.main.transform.GetChild(0));
            ps.transform.localPosition = Vector3.zero;
            ps.transform.localRotation = Quaternion.identity;
            ps.SetActive(true);
        }

        private void StopParticleSystem()
        {
            if (ps != null && ps.activeInHierarchy)
            {
                ps.SetActive(false);
                ps.transform.SetParent(ObjectPool.Instance.PooledObjectsHolder);
            }
        }

        private void StartRotation(object parameters)
        {
            GameObject star = (GameObject)parameters;
            LeanTween.rotateAroundLocal(star.gameObject, Vector3.forward, 360, 1.5f).setLoopPingPong();
            StartCongratulatoryParticleSystem();

        }

        private void OnScoreLoaded(float score)
        {
            TxtScoreValue.transform.localScale = Vector3.one;
            TxtScoreValue.text = ((int)score).ToString();
        }

        private void ScaleUpButton(GameObject Button, float delay)
        {
            LeanTween.scale(Button, Vector3.one, .25f).setDelay(delay);
        }
    }
}