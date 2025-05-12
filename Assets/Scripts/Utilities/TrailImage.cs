using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallDrop
{
    public class TrailImage : GameComponent
    {
        public MeshRenderer meshRenderer;

        public void SetTrailTexture(Texture texture)
        {
            if (meshRenderer != null)
                meshRenderer.material.SetTexture("_MainTex", texture);
            else
                Debug.LogWarning("Null mesh\nPlease Assign Mesh Renderer First");
        }

        public void Activate(Vector3 position)
        {
            LeanTween.alpha(gameObject, 1f, 0f);
            transform.position = position;
            transform.localScale = Vector3.one * 0.7f;
            Activate();
            LeanTween.alpha(gameObject, 0, .49f);
            LeanTween.scale(gameObject, Vector3.zero, .5f).setOnComplete(Deactivate);
        }
    }
}