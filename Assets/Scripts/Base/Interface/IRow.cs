using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Interfaces
{
    public interface IRow
    {
        void ActivateAndSetPosition(List<float> positions, List<CubeType> cubes, Vector3 rowPosition);

        GameObject GetGameObject();

        List<ICube> GetRowCubes();
      
        void Deactivate(bool animate);

        void OnPassedPlayer(bool animate);
    }
}