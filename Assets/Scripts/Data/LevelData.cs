using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class LevelData
    {
        public int Level;
        public int RowCount;

        private int MinRowsInALevel = 20;
        private int MaxRowsInALevel = 100;
        private int RowIncrementGap = 3;
        private int RowIncrementFactor = 2;

        public int PlusFives;
        public int PlusTens;
        public int PlusFifteens;
        public int PlusTwenty;
        public int PlusTwentyFive;

        public Dictionary<int, int> StarScoreValues;
        public int BronzeScore;
        public int SilverScore;
        public int GoldScore;

        public LevelData()
        {
            StarScoreValues = new Dictionary<int, int>();
        }

        public void ClearLevelData()
        {
            StarScoreValues.Clear();
            RowCount = 0;
            PlusFives = 0;
            PlusTens = 0;
            PlusFifteens = 0;
            PlusTwenty = 0;
            PlusTwentyFive = 0;
            BronzeScore = 0;
            SilverScore = 0;
            GoldScore = 0;
        }

        public void CalculateCurrentLevelRowCount()
        {
            RowCount = Mathf.Min((((Level / RowIncrementGap) * RowIncrementFactor) + MinRowsInALevel), MaxRowsInALevel);
        }

        public void CalculateCurrentLevelStarSystem()
        {
            int rc = RowCount;
            if (StarScoreValues != null)
                StarScoreValues.Clear();

            BronzeScore = rc * 5;
            GoldScore = 0;
            while (rc != 1)
            {
                GoldScore += rc * 5;
                rc = Mathf.FloorToInt(rc / 2.0f);
            }
            SilverScore = Mathf.FloorToInt(BronzeScore * 1.5f);
            StarScoreValues.Add(1, BronzeScore);
            StarScoreValues.Add(2, SilverScore);
            StarScoreValues.Add(3, GoldScore);
        }

        public int GetStarValue()
        {
            if (GameData.Instance.playerGameData.Score >= GoldScore)
            {
                return 3;
            }
            else if (GameData.Instance.playerGameData.Score >= SilverScore)
            {
                return 2;
            }
            else if (GameData.Instance.playerGameData.Score >= BronzeScore)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public float GetLevelProgress()
        {
            float progress = (float)GameData.Instance.playerGameData.RowsPassed / (float)RowCount;
            return progress;
        }
    }
}