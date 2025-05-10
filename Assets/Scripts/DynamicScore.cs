using BallDrop.Interfaces;
using BallDrop.Manager;
using TMPro;
using UnityEngine;

namespace BallDrop
{
    public class DynamicScore : MonoBehaviour, IDynamicScore
    {
        [SerializeField]
        private TextMeshProUGUI m_ScoreText;

        [Header("Do not set from inspector")]
        [SerializeField]
        private RectTransform Score;

        [SerializeField]
        private RectTransform m_RectTransform;

        private Vector2 Center = new Vector2(Screen.width / 2, Screen.height / 2);

        private void OnEnable()
        {
            m_RectTransform = GetComponent<RectTransform>();
            MyEventManager.Instance.EndGame.AddListener(Deactivate);
        }

        private void OnDisable()
        {
            if(MyEventManager.Instance != null)
            {
                MyEventManager.Instance.EndGame.RemoveListener(Deactivate);
            }
        }

        public void ActivateAndStartAnimation(int Increment, RectTransform parent)
        {
            m_RectTransform.SetParent(parent);
            m_ScoreText.text = "+" + Increment;
            m_RectTransform.anchoredPosition3D = parent.anchoredPosition3D;
            m_RectTransform.anchoredPosition3D = new Vector3(m_RectTransform.anchoredPosition3D.x, m_RectTransform.anchoredPosition3D.y - 250, m_RectTransform.anchoredPosition3D.z);
            Animate();

        }

        public void Deactivate()
        {
            gameObject.transform.SetParent(ObjectPool.Instance.PooledObjectsHolder);
            gameObject.SetActive(false);
            //MyEventManager.Instance.DeactivateDynamicScore.Dispatch(transform);
        }

        public void ActivateAndStartAnimation(Vector3 position, int Increment, RectTransform parent)
        {
            m_RectTransform.localPosition = position;
            m_RectTransform.SetParent(parent);
            m_ScoreText.text = "+" + Increment;
            Animate();
            //m_RectTransform.anchoredPosition3D = new Vector3(m_RectTransform.anchoredPosition3D.x, m_RectTransform.anchoredPosition3D.y - 250, m_RectTransform.anchoredPosition3D.z);

        }

        private void Animate()
        {
            m_RectTransform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            LeanTween.scale(gameObject, Vector3.one * 2, .25f).setLoopPingPong(1).setEase(LeanTweenType.easeInQuad);
            LeanTween.moveLocalY(gameObject, 0, 1f).setEaseLinear().setOnComplete(Deactivate);
        }
    }
}