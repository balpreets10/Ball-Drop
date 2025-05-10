using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallDrop
{
    public enum GameMode
    {
        Classic,
        Arcade
    }

    public enum BuildType
    {
        Test,
        Release
    }

    public class GameData : SingletonMonoBehaviour<GameData>
    {
        public LeanTweenType[] leanTweenTypes = new LeanTweenType[]
        {
            LeanTweenType.easeInOutBounce,
            LeanTweenType.easeInQuart,
            LeanTweenType.easeInCirc,
            LeanTweenType.easeInOutCubic,
            LeanTweenType.easeInSine,
        };

        public static string TAG = "BALL DROP";

        [Header("For Debugging")]
        public bool enableDeath = true;
        public bool enableReverse = true;

        [Header("Build Type")]
        public BuildType buildType = BuildType.Test;


        [Header("Game Values")]

        public GameMode gameMode;
        public Vector3 PlayerStartingPosition;
        public LevelData levelData;
        public PlayerGameData playerGameData;
        public LevelEndData levelEndData;

        private int m_Multiplier = 0;
        public int Multiplier
        {
            get { return m_Multiplier; }
        }
        private int m_MultiplierCap = 5;

        [SerializeField]
        private int ScoreIncrement = 5;

        public Vector3 rowPassPosition = Vector3.zero;

        private float m_CurrentRowMovementSpeed;
        [SerializeField]
        public float CurrentRowMovementSpeed
        {
            get { return m_CurrentRowMovementSpeed; }
            set
            {
                m_CurrentRowMovementSpeed = value;
            }
        }

        public float RowMovementSpeedCap = 5f;
        public float RowMovementSpeedIncrement = 0.1f;

        [SerializeField]
        private float m_SpaceBetweenRows = 5;

        [SerializeField]
        private float SpaceBetweenRowsCap = 7;

        public float SpaceBetweenRows
        {
            get { return m_SpaceBetweenRows; }
            set
            {
                if (value <= SpaceBetweenRowsCap)
                    m_SpaceBetweenRows = value;
                else
                    m_SpaceBetweenRows = SpaceBetweenRowsCap;
            }
        }

        public WaitForSeconds SpeedIncreaseDelay = new WaitForSeconds(1.5f);

        [Header("Player Data")]
        public Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f);

        public float PlayerMovementSensitivity = 1.0f;

        public float SwipeDetectionSensitivity;

        [Header("Ball Bounce Data")]
        [SerializeField]
        private float InitialBallBounceDistance = 3.0f;

        //Default to 3
        public float BallBounceDistance;

        public float BallBounceDistanceIncrement;

        //Default to 5;
        public float BallBounceSpeed;

        public float BallBounceSpeedIncrement;

        [HideInInspector]
        public bool IsPlayerScaled = false;

        public CameraController cameraController;

        public List<PowerupType> CurrentTypes = new List<PowerupType>();

        private void OnEnable()
        {
            MyEventManager.SetGameMode.AddListener(SetGameMode);
            MyEventManager.OnSlowDownCollected.AddListener(OnSlowDown);
            MyEventManager.OnLevelCompleted.AddListener(OnLevelCompleted);
            MyEventManager.OnRowPassed.AddListener(OnRowPassed);
            MyEventManager.OnPlayerDeath.AddListener(OnPlayerDeath);
        }

        private void OnDisable()
        {
                MyEventManager.SetGameMode.RemoveListener(SetGameMode);
                MyEventManager.OnSlowDownCollected.RemoveListener(OnSlowDown);
                MyEventManager.OnLevelCompleted.RemoveListener(OnLevelCompleted);
                MyEventManager.OnRowPassed.RemoveListener(OnRowPassed);
                MyEventManager.OnPlayerDeath.RemoveListener(OnPlayerDeath);
        }

        private void Start()
        {
            LeanTween.init(1000);
            levelData = new LevelData();
            playerGameData = new PlayerGameData();
        }

        private void SetGameMode(GameMode mode)
        {
            gameMode = mode;
            if (gameMode == GameMode.Classic)
            {
                levelData.Level = PlayerDataManager.Instance.GetPlayerLevel();
                ResetBounceDataClassic();
                ResetRowDataClassic();
                levelData.CalculateCurrentLevelRowCount();
                levelData.CalculateCurrentLevelStarSystem();
            }
            else
            {
                ResetBounceDataArcade();
                ResetRowDataArcade();
            }
            ColorData.Instance.SetColors();
            PlayerMovementSensitivity = PreferenceManager.Instance.GetIntPref(PrefKey.MovementSensitivity, 8);
            playerGameData.ClearPlayerGameData();
            ResetMultiplier();
            MySceneManager.Instance.LoadScene(Scenes.Game);
        }

        private void ResetRowDataClassic()
        {
            CurrentRowMovementSpeed = 1.0f + levelData.Level / 50.0f;
            SpaceBetweenRows = 4f + levelData.Level / 100.0f;
        }

        private void ResetRowDataArcade()
        {
            CurrentRowMovementSpeed = 1.0f;
            SpaceBetweenRows = 4f;
        }

        private void ResetBounceDataClassic()
        {
            BallBounceSpeed = 5.0f + (levelData.Level / 50.0f);
            BallBounceDistance = InitialBallBounceDistance + levelData.Level / 100.0f;
            BallBounceSpeedIncrement = 0.012f;
            if (BallBounceSpeed > 8f)
            {
                BallBounceSpeed = 8f;
                BallBounceDistance = 3f;
            }

        }

        private void ResetBounceDataArcade()
        {
            BallBounceSpeed = 5f;
            BallBounceSpeedIncrement = 0.02f;
            BallBounceDistance = InitialBallBounceDistance;
        }

        public void ResetMultiplier()
        {
            m_Multiplier = 0;
        }

        private void OnSlowDown(float duration)
        {
            StopCoroutine("StartIncreaseSpeedRowMovement");
            CurrentRowMovementSpeed -= 1.0f;
            StartCoroutine(SlowDown(duration));
        }

        private void OnPlayerDeath()
        {
            playerGameData.Score = CalculateScore();
            if (gameMode == GameMode.Classic)
            {
                levelEndData = new LevelEndData(levelData, playerGameData);
            }
            else
            {
                levelEndData = new LevelEndData(playerGameData);
            }
            levelEndData.LevelCleared = false;
            EndGame(false);

        }

        private void OnLevelCompleted()
        {
            playerGameData.Score = CalculateScore();
            levelEndData = new LevelEndData(levelData, playerGameData)
            {
                LevelCleared = true
            };
            levelData.ClearLevelData();
            EndGame(true);
        }

        private int CalculateScore()
        {
            int score = 0;
            foreach (int key in playerGameData.ConsecutiveRowsPassedCount.Keys)
            {
                score += ScoreIncrement * key * playerGameData.ConsecutiveRowsPassedCount[key];
            }
            return score;
        }

        private void EndGame(bool levelCleared)
        {
            PlayerDataManager.Instance.UpdateDataOnGameEnd(gameMode, levelEndData.Score, levelEndData.LevelCleared);
            if (levelCleared)
                StartCoroutine(EndGameWait());
            else
                MyEventManager.EndGame.Dispatch();
        }

        private IEnumerator EndGameWait()
        {
            GameObject ps = ObjectPool.Instance.GetCongratulatoryParticleSystem();
            ps.transform.position = RowSpawner.CurrentRow.transform.position;
            ps.SetActive(true);

            yield return new WaitForSeconds(2f);
            MyEventManager.EndGame.Dispatch();
            ps.SetActive(false);
            MySceneManager.Instance.LoadScene(Scenes.GameEnd);
        }

        private void OnRowPassed()
        {
            ++playerGameData.RowsPassed;
            UpdateScore();
        }

        public void UpdateScore()
        {
            if (m_Multiplier < m_MultiplierCap)
            {
                ++m_Multiplier;
            }
            int Increment = m_Multiplier * ScoreIncrement;

            if (playerGameData.ConsecutiveRowsPassedCount.ContainsKey(m_Multiplier))
                playerGameData.ConsecutiveRowsPassedCount[m_Multiplier]++;
            else
                playerGameData.ConsecutiveRowsPassedCount.Add(m_Multiplier, 1);

            MyEventManager.OnScoreUpdated.Dispatch(Increment);

        }

        public void IncreaseRowSpeed()
        {
            StartCoroutine(StartIncreaseSpeedRowMovement());
        }

        private IEnumerator StartIncreaseSpeedRowMovement()
        {
            while (true)
            {
                if (CurrentRowMovementSpeed <= RowMovementSpeedCap)
                {
                    CurrentRowMovementSpeed += RowMovementSpeedIncrement;
                    SpaceBetweenRows += RowMovementSpeedIncrement * 0.5f;
                    BallBounceSpeed += BallBounceSpeedIncrement;
                    BallBounceDistance += BallBounceDistanceIncrement;
                    yield return SpeedIncreaseDelay;
                }
                else
                    break;
            }
        }

        private IEnumerator SlowDown(float duration)
        {
            yield return new WaitForSeconds(duration);
            CurrentRowMovementSpeed += 1.0f;
            IncreaseRowSpeed();
            CurrentTypes.Remove(PowerupType.SlowDown);
        }


        public LeanTweenType GetRandomTweenType()
        {
            return (LeanTweenType)UnityEngine.Random.Range(0, leanTweenTypes.Length - 1);
        }

    }
}