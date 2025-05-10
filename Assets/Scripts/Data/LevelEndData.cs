using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class LevelEndData : LevelData
    {
        public int RowsPassed, Score;
        public bool LevelCleared;
        public Dictionary<CubeType, int> CubeData;

        public LevelEndData(PlayerGameData playerGameData)
        {
            this.CubeData = playerGameData.CubeData;
            this.RowsPassed = playerGameData.RowsPassed;
            this.Score = playerGameData.Score;
        }

        public LevelEndData(LevelData levelData, PlayerGameData playerGameData)
        {
            this.CubeData = playerGameData.CubeData;
            this.RowsPassed = playerGameData.RowsPassed;
            this.Score = playerGameData.Score;

            this.StarScoreValues = levelData.StarScoreValues;
            this.RowCount = levelData.RowCount;
            this.PlusFives = levelData.PlusFives;
            this.PlusTens = levelData.PlusTens;
            this.PlusFifteens = levelData.PlusFifteens;
            this.PlusTwenty = levelData.PlusTwenty;
            this.PlusTwentyFive = levelData.PlusTwentyFive;
            this.BronzeScore = levelData.BronzeScore;
            this.SilverScore = levelData.SilverScore;
            this.GoldScore = levelData.GoldScore;
        }
    }
}