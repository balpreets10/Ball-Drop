using BallDrop;
using BallDrop.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeRowData : MonoBehaviour, IRowData
{
    private int MaxCubes = 6;
    private int MinCubes = 3;
    private int thisRowCubeCount;
    private int RowCubeCapacity = 7;
    private int DifficultyFactor = 17;
    public int TestDifficulty = 0;

    private int CurrentRowNum;

    private List<int> zValuesCurrentRow = new List<int>();
    private List<int> AvailableZValues = new List<int>();
    private List<CubeType> CubesCurrentRow = new List<CubeType>();

    private int ShieldActivationCounter = 0;
    private int SlowDownActivationCounter = 0;
    private int VerticalBeamActivationCounter = 0;
    private int CoinRowCount = 0;

    private void Start()
    {
        CoinRowCount = UnityEngine.Random.Range(5, 11);
        SlowDownActivationCounter = UnityEngine.Random.Range(15, 30);
        ShieldActivationCounter = UnityEngine.Random.Range(25, 40);
        VerticalBeamActivationCounter = UnityEngine.Random.Range(40, 65);
    }

    public void ActivatePowerUp(int CurrentRowCounter, Vector3 pos)
    {
        if (CurrentRowCounter == SlowDownActivationCounter)
        {
            if (!GameData.Instance.CurrentTypes.Contains(PowerupType.SlowDown) && GameData.Instance.CurrentRowMovementSpeed > 1.9f)
            {
                IPowerup powerup = ObjectPool.Instance.GetSlowDownPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(pos);
                if (CurrentRowCounter == ShieldActivationCounter)
                    ShieldActivationCounter += 3;
                if (CurrentRowCounter == VerticalBeamActivationCounter)
                    VerticalBeamActivationCounter += 5;
            }
            SlowDownActivationCounter += UnityEngine.Random.Range(20, 30);
        }
        if (CurrentRowCounter == ShieldActivationCounter)
        {
            if (!GameData.Instance.CurrentTypes.Contains(PowerupType.Shield))
            {
                IPowerup powerup = ObjectPool.Instance.GetShieldPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(pos);
                if (CurrentRowCounter == VerticalBeamActivationCounter)
                    VerticalBeamActivationCounter += 4;

            }
            ShieldActivationCounter += UnityEngine.Random.Range(25, 40);
        }
        if (CurrentRowCounter == VerticalBeamActivationCounter)
        {
            IPowerup powerup = ObjectPool.Instance.GetVerticalBeamPowerup();
            powerup.ActivateAndSetPosition(pos);
            VerticalBeamActivationCounter += UnityEngine.Random.Range(30, 50);
        }
    }

    public void ActivateCoin(int CurrentRowCounter, List<float> positions, Vector3 currentSpawnPosition, Transform parent)
    {
        if (CoinRowCount == CurrentRowCounter)
        {
            for (int i = 0; i < 7; i++)
            {
                if (!positions.Contains(i))
                {
                    CoinBase coin = ObjectPool.Instance.GetCoin();
                    coin.Activate(new Vector3(currentSpawnPosition.x, currentSpawnPosition.y, i), Quaternion.identity, parent);
                    CoinRowCount += UnityEngine.Random.Range(7, 14);
                    break;
                }
            }
        }
    }

    public Dictionary<float, CubeType> GetRowData(int rowNum, int level)
    {
        CurrentRowNum = rowNum;
        thisRowCubeCount = Mathf.Min(MaxCubes, 3 + Mathf.FloorToInt(rowNum * 0.05f));
        List<int> positions = GetRandomPositions();
        List<CubeType> cubes = GetRandomCubes(rowNum);
        Dictionary<float, CubeType> dictionary = new Dictionary<float, CubeType>();
        for (int i = 0; i < positions.Count; i++)
        {
            dictionary.Add(positions[i], cubes[i]);
        }
        return dictionary;
    }

    private List<int> GetRandomPositions()
    {
        zValuesCurrentRow.Clear();
        if (AvailableZValues.Count != 0)
        {
            ShuffleAvailableZValues();
            for (int i = 0; i < AvailableZValues.Count; i++)
            {
                if (zValuesCurrentRow.Count < thisRowCubeCount)
                {
                    zValuesCurrentRow.Add(AvailableZValues[i]);
                }
                else
                    break;
            }
        }
        while (zValuesCurrentRow.Count < thisRowCubeCount)
        {
            int pos = UnityEngine.Random.Range(0, (int)CubeType.Landing);
            if (zValuesCurrentRow.Contains(pos))
                continue;
            else
                zValuesCurrentRow.Add(pos);
        }

        AvailableZValues.Clear();
        if (zValuesCurrentRow != null)
            for (int i = 0; i <= RowCubeCapacity; i++)
            {
                if (!zValuesCurrentRow.Contains(i))
                    AvailableZValues.Add(i);
            }

        return zValuesCurrentRow;
    }

    private void ShuffleAvailableZValues()
    {
        for (int i = 0; i < AvailableZValues.Count; i++)
        {
            int tmp = AvailableZValues[i];
            int r = UnityEngine.Random.Range(i, AvailableZValues.Count - 1);
            AvailableZValues[i] = AvailableZValues[r];
            AvailableZValues[r] = tmp;
        }
    }

    private int GetRandom(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private List<CubeType> GetRandomCubes(int CurrentRowCounter)
    {
        CubesCurrentRow.Clear();
        int difficulty;
        if (TestDifficulty != 0)
            difficulty = TestDifficulty;
        else
            difficulty = DifficultyFactor;

        int newCube;
        int limit = CurrentRowNum % difficulty;
        float prob = (float)limit / difficulty;

        while (CubesCurrentRow.Count < thisRowCubeCount)
        {
            if (CurrentRowCounter < 2)
            {
                CubesCurrentRow.Add(CubeType.Normal);
            }
            else
            {
                if (CubesCurrentRow.Count < 2)
                {
                    CubesCurrentRow.Add(CubeType.Normal);
                }
                else
                {

                    if (difficulty * (Enum.GetValues(typeof(CubeType)).Length - 1) > CurrentRowNum)
                    {
                        float random = UnityEngine.Random.Range(0f, 1f);
                        if (random <= prob)
                            newCube = Mathf.FloorToInt(CurrentRowNum / difficulty);
                        else
                            newCube = GetRandom(0, (Mathf.FloorToInt(CurrentRowNum / difficulty)));
                    }
                    else
                        newCube = GetRandom(0, Enum.GetValues(typeof(CubeType)).Length - 1);

                    CubesCurrentRow.Add((CubeType)newCube);
                }
            }
        }
        return CubesCurrentRow;

    }
}
