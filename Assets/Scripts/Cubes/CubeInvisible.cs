using BallDrop.Base;
using BallDrop.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeInvisible : BaseCube
    {
        Coroutine coroutine;

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            transform.SetPositionAndRotation(new Vector3(parent.position.x, parent.position.y, zpos), Quaternion.identity);
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(true);
            coroutine = StartCoroutine(ChangeCube());
        }

        public override void Deactivate()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            gameObject.SetActive(false);
        }

        public override void FlipAndDeactivate()
        {
            Deactivate();
        }

        private IEnumerator ChangeCube()
        {
            RowBase parent = transform.parent.GetComponent<RowBase>();
            while (parent.transform.position.y < GameData.Instance.cameraController.MainCamera.transform.position.y - 20)
            {
                yield return null;
            }
            parent.ChangeInvisibleCube(transform.position.z);
        }
    }
}