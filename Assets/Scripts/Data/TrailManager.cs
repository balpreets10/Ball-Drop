using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class TrailManager : MonoBehaviour
    {
        public List<Texture> TrailImages;
        public int index;

        private void OnEnable()
        {
            MyEventManager.UpdateLockStatus.AddListener(OnTrailChanged);
        }

        private void OnDisable()
        {
            MyEventManager.UpdateLockStatus.RemoveListener(OnTrailChanged);
        }

        private void OnTrailChanged(int index, ItemType itemType, LockStatus lockStatus)
        {
            if (itemType == ItemType.Trail && lockStatus == LockStatus.Selected)
            {
                this.index = index;
                List<TrailImage> trails = ObjectPool.Instance.GetTrailsList();

                foreach (TrailImage trail in trails)
                {
                    trail.SetTrailTexture(TrailImages[index]);
                }
            }
        }
    }
}