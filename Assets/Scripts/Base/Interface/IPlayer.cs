using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop.Interfaces
{
    public interface IPlayer
    {
        void ActivateAndSetPosition();

        void Deactivate();
    }
}