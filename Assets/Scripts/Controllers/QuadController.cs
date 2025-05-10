using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class QuadController : MonoBehaviour
    {
        [Range(-0.01f, -0.3f)]
        public float speed;
        public Vector2 offset;
        public MeshRenderer meshRenderer;
        public List<Color> color;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            MyEventManager.OnBackgroundUpdated.AddListener(OnBackgroundUpdated);
        }

        private void OnDisable()
        {
                MyEventManager.OnBackgroundUpdated.RemoveListener(OnBackgroundUpdated);

        }

        private void Update()
        {
            offset = new Vector2(0, Time.time * speed);
            meshRenderer.material.mainTextureOffset = offset;
        }

        private void OnBackgroundUpdated(Texture sprite)
        {
            meshRenderer.material.SetTexture("_MainTex", sprite);
        }

    }

}