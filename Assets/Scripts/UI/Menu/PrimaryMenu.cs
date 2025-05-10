using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class PrimaryMenu : MonoBehaviour
    {
        public virtual void Start()
        {
            MySceneManager.Instance.HideLoadingCanvas();
        }
    }
}