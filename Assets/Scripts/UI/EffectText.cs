using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallDrop
{
    [RequireComponent(typeof(Billboard))]
    public class EffectText : UIComponent
    {
        public TextMeshProUGUI Text;
        private RectTransform rectTransform;
        public bool activeInHierarchy { get { return gameObject.activeInHierarchy; } }

        protected override void Awake()
        {
            base.Awake();
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        }

        public void Activate(Transform parent, Vector3 position, string textValue)
        {
            Activate();
            SetParent(parent);
            SetText(textValue);
            SetRect(position);
            LeanTween.scale(gameObject, Vector3.one * 2f, .75f).setOnComplete(Deactivate).setEase(LeanTweenType.linear);
            LeanTween.moveLocalY(gameObject, rectTransform.anchoredPosition3D.y + 200, .75f);
        }

        private void SetText(string textValue)
        {
            if (Text != null)
            {
                Text.text = textValue;
                Text.color = ColorData.Instance.GetSecondaryColor();
                Text.outlineWidth = .4f;
                Text.outlineColor = ColorData.Instance.GetPrimaryColor();
            }
        }

        private void SetRect(Vector3 position)
        {
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition3D = position;
                rectTransform.localRotation = Quaternion.identity;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SetParent(ObjectPool.Instance.PooledObjectsHolder);
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}