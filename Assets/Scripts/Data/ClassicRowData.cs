using System.Collections;
using System.Collections.Generic;
using BallDrop;
using BallDrop.Interfaces;
using UnityEngine;
using System.Linq;
using System;

public class ClassicRowData : MonoBehaviour, IRowData
{
    private int CoinRowCount = 0;
    [SerializeField]
    private float ThreeCubesWeight = 0.1875f;
    [SerializeField]
    private float FourFiveCubesWeight = 0.25f;
    [SerializeField]
    private float SevenCubesWeight = 0.125f;
    private int thisRowCubeCount = 3;
    private int nextKey = 2;
    private CubeType limitCube = CubeType.Normal;

    private float CurrentLevelProbability;
    private int ShieldActivationCounter = 0;
    private int SlowDownActivationCounter = 0;
    private int VerticalBeamActivationCounter = 0;

    private Dictionary<int, int> RowCubeCounts;
    private List<int> zValuesCurrentRow = new List<int>();
    private List<int> AvailableZValues = new List<int>();
    private List<CubeType> CubesCurrentRow = new List<CubeType>();
    private Dictionary<int, CubeType> CubeStartValues = new Dictionary<int, CubeType>
    {
            { 3,CubeType.Enemy},
            { 10,CubeType.Faded },
            { 20,CubeType.Moving },
            { 33,CubeType.X },
            { 48,CubeType.Spike },
            { 68,CubeType.Reverse },
            { 90,CubeType.Invisible }
    };


    void Start()
    {
        int rowCount = GameData.Instance.levelData.RowCount;
        RowCubeCounts = new Dictionary<int, int>
        {
            { 3, Mathf.CeilToInt(rowCount * ThreeCubesWeight) }
        };
        RowCubeCounts.Add(4, RowCubeCounts[3] + Mathf.FloorToInt(rowCount * FourFiveCubesWeight));
        RowCubeCounts.Add(5, RowCubeCounts[4] + Mathf.FloorToInt(rowCount * FourFiveCubesWeight));
        RowCubeCounts.Add(6, rowCount - 1 - Mathf.FloorToInt(rowCount * SevenCubesWeight));

        foreach (int key in CubeStartValues.Keys)
        {
            if (GameData.Instance.levelData.Level >= key)
            {
                limitCube = CubeStartValues[key];
                if ((int)limitCube == Enum.GetValues(typeof(CubeType)).Length - 2)
                    nextKey = 100;
                else
                    nextKey = CubeStartValues.Keys.ToList()[(int)limitCube];
            }
            else
                break;
        }
        CurrentLevelProbability = (float)GameData.Instance.levelData.Level / ((nextKey - 1) * 2);


        SlowDownActivationCounter = UnityEngine.Random.Range(rowCount / 3, rowCount - 3);
        ShieldActivationCounter = UnityEngine.Random.Range(rowCount / 3, rowCount - 3);
        VerticalBeamActivationCounter = UnityEngine.Random.Range(rowCount / 2, rowCount - 5);
        CoinRowCount = UnityEngine.Random.Range(5, 11);

    }

    public void ActivatePowerUp(int CurrentRowCounter, Vector3 pos)
    {
        if (CurrentRowCounter == SlowDownActivationCounter && GameData.Instance.levelData.Level > 5)
        {
            if (GameData.Instance.levelData.RowCount - CurrentRowCounter >= 25)
                SlowDownActivationCounter += UnityEngine.Random.Range(20, GameData.Instance.levelData.RowCount - 3);
            if (UnityEngine.Random.Range(0, 2) == 1 && GameData.Instance.CurrentRowMovementSpeed > 1.9f)
            {
                IPowerup powerup = ObjectPool.Instance.GetSlowDownPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(pos);
                if (CurrentRowCounter == ShieldActivationCounter)
                    ShieldActivationCounter += 3;
                if (CurrentRowCounter == VerticalBeamActivationCounter)
                    VerticalBeamActivationCounter += 5;
            }
        }
        if (CurrentRowCounter == ShieldActivationCounter && GameData.Instance.levelData.Level > 3)
        {
            if (GameData.Instance.levelData.RowCount - CurrentRowCounter >= 30)
                ShieldActivationCounter += UnityEngine.Random.Range(20, GameData.Instance.levelData.RowCount - 3);
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                IPowerup powerup = ObjectPool.Instance.GetShieldPowerUp().GetComponent<IPowerup>();
                powerup.ActivateAndSetPosition(pos);
                if (CurrentRowCounter == VerticalBeamActivationCounter)
                    VerticalBeamActivationCounter += 4;
            }
        }
        if (CurrentRowCounter == VerticalBeamActivationCounter && GameData.Instance.levelData.Level > 10)
        {
            if (GameData.Instance.levelData.RowCount - CurrentRowCounter >= 30)
                ShieldActivationCounter += UnityEngine.Random.Range(22, GameData.Instance.levelData.RowCount - 4);
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                IPowerup powerup = ObjectPool.Instance.GetVerticalBeamPowerup();
                powerup.ActivateAndSetPosition(pos);
            }
        }
    }

    public void ActivateCoin(int CurrentRowCounter, List<float> positions, Vector3 currentSpawnPosition, Transform parent)
    {
        if (CoinRowCount == CurrentRowCounter && CoinRowCount < GameData.Instance.levelData.RowCount - 2)
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
        if (rowNum < GameData.Instance.levelData.RowCount)
        {
            if (thisRowCubeCount < 7 && rowNum > RowCubeCounts[thisRowCubeCount])
                thisRowCubeCount++;

            List<int> positions = GetRandomPositions();
            List<CubeType> cubes = GetRandomCubes(rowNum);
            Dictionary<float, CubeType> dictionary = new Dictionary<float, CubeType>();
            for (int i = 0; i < positions.Count; i++)
            {
                dictionary.Add(positions[i], cubes[i]);
            }
            return dictionary;
        }
        else if (rowNum == GameData.Instance.levelData.RowCount)
        {
            return new Dictionary<float, CubeType>
            {
                { 3.5f, CubeType.Landing }
            };
        }
        else
        {
            return null;
        }
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
            int pos = UnityEngine.Random.Range(0, 7);
            if (zValuesCurrentRow.Contains(pos))
                continue;
            else
                zValuesCurrentRow.Add(pos);
        }

        AvailableZValues.Clear();
        if (zValuesCurrentRow != null)
            for (int i = 0; i < 8; i++)
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

    private List<CubeType> GetRandomCubes(int CurrentRowCounter)
    {
        CubesCurrentRow.Clear();

        while (CubesCurrentRow.Count < thisRowCubeCount)
        {
            if (CurrentRowCounter < 2 || limitCube == CubeType.Normal)
            {
                CubesCurrentRow.Add(CubeType.Normal);
            }
            else
            {
                if (CubesCurrentRow.Count < 2)
                {
                    CubesCurrentRow.Add(CubeType.Normal);
                }
                else if (thisRowCubeCount > 5 && CubesCurrentRow.Count < 3 && limitCube == CubeType.Faded)
                {
                    CubesCurrentRow.Add(CubeType.Faded);
                }
                else
                {
                    if (GameData.Instance.levelData.Level < 101)
                    {
                        float random = UnityEngine.Random.Range(0.1f, 0.9f);
                        if (random <= CurrentLevelProbability)
                        {
                            CubesCurrentRow.Add(limitCube);
                        }
                        else if (random > 0.7f)
                        {
                            CubeType cube = (CubeType)UnityEngine.Random.Range(0, (int)limitCube);
                            CubesCurrentRow.Add(cube);
                        }
                        else
                        {
                            CubeType cube = (CubeType)Mathf.Min((int)limitCube - 1, thisRowCubeCount - 2);
                            CubesCurrentRow.Add(cube);
                        }

                    }
                    else
                        CubesCurrentRow.Add((CubeType)UnityEngine.Random.Range(0, (int)limitCube + 1));
                }
            }
        }
        return CubesCurrentRow;
    }

}


