using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace BallDrop
{
    [RequireComponent(typeof(Billboard))]
    public class EffectText : MonoBehaviour
    {

        public TextMeshProUGUI Text;
        private RectTransform rectTransform;
        public bool activeInHierarchy { get { return gameObject.activeInHierarchy; } }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Activate(Transform parent, Vector3 position, string Text)
        {
            gameObject.SetActive(true);
            SetParent(parent);
            this.Text.text = Text;
            this.Text.color = ColorData.Instance.GetSecondaryColor();
            this.Text.outlineWidth = .4f;
            this.Text.outlineColor = ColorData.Instance.GetPrimaryColor();
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition3D = position;
            LeanTween.scale(gameObject, Vector3.one * 2f, .75f).setOnComplete(Deactivate).setEase(LeanTweenType.linear);
            LeanTween.moveLocalY(gameObject, rectTransform.anchoredPosition3D.y + 200, .75f);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
            SetParent(ObjectPool.Instance.PooledObjectsHolder);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }

}