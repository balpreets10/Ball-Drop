using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class SplatterManager : MonoBehaviour
    {
        public List<BaseSplatter> Splatters;
        public int index;

        private void OnEnable()
        {
            MyEventManager.Instance.UpdateLockStatus.AddListener(OnSplatterChanged);

        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.UpdateLockStatus.RemoveListener(OnSplatterChanged);

            }

        }

        private void OnSplatterChanged(int index, ItemType itemType, LockStatus lockStatus)
        {
            if (itemType == ItemType.Splatter && lockStatus == LockStatus.Selected)
            {
                this.index = index;
                //List<BaseSplatters> trails = ObjectPool.Instance.GetTrailsList();

                //foreach (TrailImage trail in trails)
                //{
                //    trail.SetTrailTexture(TrailImages[index]);
                //}
            }
        }
    }
}