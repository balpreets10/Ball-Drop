using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Interfaces
{
    public interface IDynamicScore
    {
        void ActivateAndStartAnimation(int Increment, RectTransform parent);
        void ActivateAndStartAnimation(Vector3 position, int Increment, RectTransform parent);

        void Deactivate();
    }
}