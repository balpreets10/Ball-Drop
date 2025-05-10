using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    // If you change Cubetype enum values update it in BaseCube Class as well.

    public enum CubeType
    {
        Normal,
        Enemy,
        Faded,
        Moving,
        X,
        Spike,
        Reverse,
        Invisible,
        Landing,
    }
}