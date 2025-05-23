﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class RevealEffect : UIComponent
    {
        public Image Circle;

        private void OnEnable()
        {
            MyEventManager.Game.StartGame.AddListener(StartGame);
            MyEventManager.Reveal.AddListener(Reveal);
            Circle.transform.localScale = Vector3.zero;
        }

        private void OnDisable()
        {
            MyEventManager.Game.StartGame.RemoveListener(StartGame);
            MyEventManager.Reveal.RemoveListener(Reveal);
        }

        private void Reveal()
        {
            gameObject.SetActive(true);
            Circle.transform.localScale = Vector3.zero;
            LeanTween.scale(Circle.gameObject, Vector3.one * 50, .4f).setLoopPingPong(1).setOnComplete(Deactivate);
        }

        private void StartGame()
        {
            Circle.color = ColorData.Instance.GetPrimaryColor();
            Reveal();
        }
    }
}