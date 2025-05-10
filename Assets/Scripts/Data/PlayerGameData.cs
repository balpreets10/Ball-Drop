using BallDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameData
{
    public int RowsPassed;
    public int Score;

    public Dictionary<int, int> ConsecutiveRowsPassedCount;
    public Dictionary<CubeType, int> CubeData;

    public PlayerGameData()
    {
        ConsecutiveRowsPassedCount = new Dictionary<int, int>();
        CubeData = new Dictionary<CubeType, int>();
        RowsPassed = 0;
        AddCubeTypes();
    }

    private void AddCubeTypes()
    {
        CubeData.Add(CubeType.Normal, 0);
        CubeData.Add(CubeType.Enemy, 0);
        CubeData.Add(CubeType.Faded, 0);
        CubeData.Add(CubeType.Moving, 0);
        CubeData.Add(CubeType.X, 0);
        CubeData.Add(CubeType.Reverse, 0);
        CubeData.Add(CubeType.Spike, 0);
    }

    public void AddCubeData(CubeType cubeType)
    {
        if (cubeType != CubeType.Landing && cubeType != CubeType.Invisible)
            CubeData[cubeType] = CubeData[cubeType] + 1;
    }

    public Dictionary<CubeType, int> GetCubeData()
    {
        return CubeData;
    }

    public void ClearPlayerGameData()
    {
        CubeData.Clear();
        ConsecutiveRowsPassedCount.Clear();
        AddCubeTypes();
        Score = 0;
        RowsPassed = 0;
    }
}
