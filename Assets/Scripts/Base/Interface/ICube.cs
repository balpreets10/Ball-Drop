using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Interfaces
{
    public interface ICube
    {
        CubeType GetBaseCubeType();

        void SetBaseCubeType(CubeType cubeType);

        float GetEffectDuration();

        void SetTexture(Texture cubeTexture);

        void SetColor(Color color);

        void ActivateAndSetPosition(Transform parent, float zpos);

        void FlipAndDeactivate();

        void Deactivate();

        void PlayBounceSound();

        GameObject GetGameObject();

        Transform GetTransform();

        RectTransform GetRectTransform();
    }
}