using System;
using System.Collections.Generic;

namespace BallDrop
{
    [Serializable]
    public class PlayerData
    {
        public string Name;
        public int Coins;
        public int ArcadeScore;
        public int ClassicScore;
        public int CurrentLevel;
        public int TotalSeen;
        public DateTime LastWatchedTime;

        public static readonly string MinDate = "01-01-0001 00:00:00";
        public static readonly string DTFormat = "dd-MM-yyyy h:mm:ss";

        #region To be used later
        //public TrailData trailData;
        //public SplatterData splatterData;
        #endregion
    }


    #region FUTURE UPDATES
    public enum LockStatus
    {
        Locked,
        Unlocked,
        Selected
    }

    [Serializable]
    public class TrailData
    {
        public List<LockStatus> trailStatus;

        public TrailData()
        {
            trailStatus = new List<LockStatus>
            {
                LockStatus.Selected
            };
            for (int i = 0; i < 20; i++)
                trailStatus.Add(LockStatus.Locked);
        }
    }

    [Serializable]
    public class SplatterData
    {
        public List<LockStatus> SplatterStatus;

        public SplatterData()
        {
            SplatterStatus = new List<LockStatus>
            {
                LockStatus.Selected
            };
            for (int i = 0; i < 20; i++)
                SplatterStatus.Add(LockStatus.Locked);
        }
    }
    #endregion

}