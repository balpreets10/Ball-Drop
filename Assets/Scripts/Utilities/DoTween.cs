using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
namespace BallDrop
{
    public class DoTween : MonoBehaviour
    {
        public bool TweenOnStart = true;

        public enum TweenType
        {
            Move,
            Rotate,
            Scale
        }

        public enum TweenMode
        {
            Once,
            Loop,
            LoopOnce,
            LoopCount,
            PingPong,
            PingPongOnce,
            PingPongCount
        }

        MethodInfo infoInt;
        MethodInfo infoFloat;
        MethodInfo infoString;
        MethodInfo infoBool;
        MethodInfo info;
        public float TweenTime;

        public TweenType tween;
        [Header("Movement Properties")]
        public Vector3 Position;

        [Header("Rotation Properties")]
        public Vector3 Axes;
        public float RotationAdd;

        [Header("Scale Properties")]
        public Vector3 StartScale;
        public Vector3 FinalScale;

        [Header("General Tween Properties")]
        public TweenMode tweenMode;
        public int LoopCount = 1;

        public LeanTweenType TweenEase;

        LTDescr tweenObject = null;


        [Header("Tween Events")]
        public UnityEvent OnTweenStart;
        public float OnStartDelay = 0f;
        public UnityEvent OnTweenComplete;
        public float OnCompleteDelay = 0f;

        private void Start()
        {
            if (TweenOnStart)
            {
                PerformTween();
            }
        }

        private void PerformTween()
        {
            LeanTween.cancel(gameObject);
            switch (tween)
            {
                case TweenType.Move:
                    PerformMovementTween();
                    break;
                case TweenType.Rotate:
                    PwerformRotationTween();
                    break;
                case TweenType.Scale:
                    PerformScaleTween();
                    break;
            }
        }

        public void StartTween()
        {
            PerformTween();
        }

        private void OnDisable()
        {
            if (tweenObject != null)
                LeanTween.cancel(tweenObject.id);
        }

        private void PerformScaleTween()
        {
            transform.localScale = StartScale;
            tweenObject = LeanTween.scale(gameObject, FinalScale, TweenTime).setEase(TweenEase);
            SetupTween(tweenObject);
        }

        private void PwerformRotationTween()
        {
            tweenObject = LeanTween.rotateAroundLocal(gameObject, Axes, RotationAdd, TweenTime).setEase(TweenEase);
            SetupTween(tweenObject);
        }

        private void PerformMovementTween()
        {
            if (Position.x > 0)
                tweenObject = LeanTween.moveLocalX(gameObject, Position.x, TweenTime).setEase(TweenEase);
            else if (Position.y > 0)
                tweenObject = LeanTween.moveLocalY(gameObject, Position.y, TweenTime).setEase(TweenEase);
            else if (Position.z > 0)
                tweenObject = LeanTween.moveLocalZ(gameObject, Position.z, TweenTime).setEase(TweenEase);

            SetupTween(tweenObject);

        }

        private void SetupTween(LTDescr tweenObject)
        {
            if (OnTweenStart.GetPersistentEventCount() > 0)
            {
                for (int i = 0; i < OnTweenStart.GetPersistentEventCount(); i++)
                {
                    Invoke(OnTweenStart.GetPersistentMethodName(i), OnStartDelay);
                }
            }
            switch (tweenMode)
            {
                case TweenMode.Once:
                    break;
                case TweenMode.Loop:
                    tweenObject.setLoopType(TweenEase);
                    break;
                case TweenMode.LoopCount:
                    tweenObject.setLoopCount(LoopCount);
                    break;
                case TweenMode.LoopOnce:
                    tweenObject.setLoopCount(1);
                    break;
                case TweenMode.PingPong:
                    tweenObject.setLoopPingPong();
                    break;
                case TweenMode.PingPongOnce:
                    tweenObject.setLoopPingPong(1);
                    break;
                case TweenMode.PingPongCount:
                    tweenObject.setLoopPingPong(LoopCount);
                    break;
            }

            tweenObject.setOnComplete(OnTweenFiished);

        }

        private void OnTweenFiished()
        {
            if (OnTweenComplete.GetPersistentEventCount() > 0)
            {
                for (int i = 0; i < OnTweenComplete.GetPersistentEventCount(); i++)
                {
                    infoInt = UnityEventBase.GetValidMethodInfo(OnTweenComplete.GetPersistentTarget(i), OnTweenComplete.GetPersistentMethodName(i), new Type[] { typeof(int) });
                    infoString = UnityEventBase.GetValidMethodInfo(OnTweenComplete.GetPersistentTarget(i), OnTweenComplete.GetPersistentMethodName(i), new Type[] { typeof(string) });
                    infoFloat = UnityEventBase.GetValidMethodInfo(OnTweenComplete.GetPersistentTarget(i), OnTweenComplete.GetPersistentMethodName(i), new Type[] { typeof(float) });
                    infoBool = UnityEventBase.GetValidMethodInfo(OnTweenComplete.GetPersistentTarget(i), OnTweenComplete.GetPersistentMethodName(i), new Type[] { typeof(bool) });
                    info = UnityEventBase.GetValidMethodInfo(OnTweenComplete.GetPersistentTarget(i), OnTweenComplete.GetPersistentMethodName(i), new Type[] { });

                    if (infoInt != null)
                        infoInt.Invoke(OnTweenComplete.GetPersistentTarget(i), new Type[] { typeof(int) });
                    if (infoString != null)
                        infoString.Invoke(OnTweenComplete.GetPersistentTarget(i), new Type[] { typeof(string) });
                    if (infoFloat != null)
                        infoFloat.Invoke(OnTweenComplete.GetPersistentTarget(i), new Type[] { typeof(float) });
                    if (infoBool != null)
                        infoBool.Invoke(OnTweenComplete.GetPersistentTarget(i), new Type[] { typeof(bool) });
                    if (info != null)
                        info.Invoke(OnTweenComplete.GetPersistentTarget(i), new Type[] { });


                    //Invoke(OnTweenComplete.GetPersistentTarget(i) OnTweenComplete.GetPersistentMethodName(i),OnCompleteDelay);
                }
            }


        }
        public void SetCustomTweenData(TweenType tweenType, params object[] data)
        {
            switch (tweenType)
            {
                case TweenType.Move:
                    Position = (Vector3)data[0];
                    break;
                case TweenType.Rotate:
                    Axes = (Vector3)data[0];
                    RotationAdd = (float)data[1];
                    break;
                case TweenType.Scale:
                    StartScale = (Vector3)data[0];
                    FinalScale = (Vector3)data[1];
                    break;
            }
        }

        public List<object> GetTweenData(TweenType tweenType)
        {
            List<object> tweenData = new List<object>();
            switch (tweenType)
            {
                case TweenType.Move:
                    tweenData.Add(Position);
                    break;
                case TweenType.Rotate:
                    tweenData.Add(Axes);
                    tweenData.Add(RotationAdd);
                    break;
                case TweenType.Scale:
                    tweenData.Add(StartScale);
                    tweenData.Add(FinalScale);
                    break;
            }
            return tweenData;
        }

    }
}