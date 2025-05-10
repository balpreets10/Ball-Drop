using BallDrop.Audio;
using BallDrop.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeMoving : BaseCube
    {
        [Header("Moving Cube Properties")]
        public float MoveSpeed = .5f;

        public float TimeToMove;
        public int Distance;

        public override void Deactivate()
        {
            //LeanTween.cancel(gameObject);
            base.Deactivate();
        }

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            //Debug.Log("Activating moving");
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            LeanTweenType motionType = GameData.Instance.GetRandomTweenType();
            TimeToMove = UnityEngine.Random.Range(1, 2);
            Distance = UnityEngine.Random.Range(1, 5);
            LTDescr lTDescr = LeanTween.moveZ(gameObject, Distance, TimeToMove).setLoopType(motionType).setEase(motionType);
            lTDescr.setLoopPingPong();
        }
    }
}