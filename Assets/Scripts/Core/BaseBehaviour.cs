using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BallDrop
{
    public class BaseBehaviour : MonoBehaviour
    {
        public virtual void Activate()
        {
        }

        public virtual void Deactivate()
        {
        }

        protected virtual void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void Start()
        {
        }
    }
}