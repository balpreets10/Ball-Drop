using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class BackgroundsController : MonoBehaviour
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


        //private void OnBackgroundUpdated(Texture sprite)
        //{
        //    if (GetComponent<Canvas>().worldCamera == null)
        //        GetComponent<Canvas>().worldCamera = GetComponentInChildren<Camera>();
        //    Background.color = color[UnityEngine.Random.Range(0, color.Count)];
        //}

        private void OnBackgroundUpdated(Texture sprite)
        {

            Background.material.SetTexture("_MainTex", sprite);
        }
    }
}