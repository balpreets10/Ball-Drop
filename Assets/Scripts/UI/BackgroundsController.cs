using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class BackgroundsController : UIComponent
    {
        [SerializeField]
        private MeshRenderer Background;

        public List<Color> color;

        private void OnEnable()
        {
            MyEventManager.OnBackgroundUpdated.AddListener(OnBackgroundUpdated);
        }

        private void OnDisable()
        {
            MyEventManager.OnBackgroundUpdated.RemoveListener(OnBackgroundUpdated);
        }

        private void OnBackgroundUpdated(Texture sprite)
        {
            Background.material.SetTexture("_MainTex", sprite);
        }
    }
}