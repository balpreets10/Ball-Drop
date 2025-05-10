using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class BaseSplatter : MonoBehaviour
    {
        private Transform PoolParent;
        public bool activeInHierarchy
        {
            get { return gameObject.activeInHierarchy; }
        }
        public virtual void Activate(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            LeanTween.moveLocalY(gameObject, .6f, 0f);
        }

        public virtual void ActivateAndSetParent(Vector3 position, Transform parent)
        {
            transform.position = position;
            transform.SetParent(parent);
            Activate(position);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            transform.SetParent(PoolParent);
            transform.position = Vector3.zero;
        }

        public void UpdateParentObject(Transform PoolParent)
        {
            this.PoolParent = PoolParent;
        }
    }
}