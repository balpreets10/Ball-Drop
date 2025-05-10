using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}