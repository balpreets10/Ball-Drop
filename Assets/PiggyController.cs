using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class PiggyController : MonoBehaviour
    {
        public GameObject Coin;
        Vector3 coinInitial;

        private void Start()
        {
            coinInitial = Coin.transform.localPosition;
        }

        private void OnEnable()
        {
            MyEventManager.EndGame.AddListener(EndGame);
        }

        private void OnDisable()
        {
                MyEventManager.EndGame.RemoveListener(EndGame);
        }

        public void Animate()
        {
            LeanTween.moveLocalX(gameObject, -625, 0.5f).setOnComplete(AnimateCoin);
        }

        private void AnimateCoin()
        {
            LeanTween.moveLocalY(Coin, Coin.transform.localPosition.y - 100, 0.5f).setOnComplete(OnCompleted);
        }

        private void OnCompleted()
        {
            LeanTween.moveLocalX(gameObject, -920, 0.5f).setOnComplete(ResetCoin);
        }

        private void ResetCoin()
        {
            Coin.transform.localPosition = coinInitial;
        }

        private void EndGame()
        {
            LeanTween.cancel(gameObject);
        }

    }
}