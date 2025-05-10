using BallDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRowData
{
    Dictionary<float, CubeType> GetRowData(int rowNum, int level);

    void ActivatePowerUp(int CurrentRowCounter, Vector3 pos);

    void ActivateCoin(int CurrentRowCounter, List<float> positions, Vector3 currentSpawnPosition, Transform parent);

}
