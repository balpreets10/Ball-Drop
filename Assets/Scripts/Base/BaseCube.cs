using BallDrop.Audio;
using BallDrop.Interfaces;
using System;
using UnityEngine;

namespace BallDrop.Base
{
    public class BaseCube : GameComponent, ICube
    {
        public CubeType m_BaseCubeType;
        public MeshRenderer m_Renderer;
        public BoxCollider m_BoxCollider;

        [Header("Cubes that effect Gameplay")]
        public float EffectDuration;

        [Header("Audio Properties")]
        public AudioClip m_BounceSoundNormal;

        public AudioClip m_BounceSoundHeavy;
        private RectTransform m_RectTransform;
        public Vector3 InitialScale;

        protected override void Awake()
        {
            base.Awake();
            m_RectTransform = GetComponent<RectTransform>();
            InitialScale = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                ObjectPool.Instance.GetBreakParticle().Activate(transform.position, ColorData.Instance.GetPrimaryColor());
                transform.localScale = Vector3.zero;
            }
        }

        public virtual void ActivateAndSetPosition(Transform parent, float zpos)
        {
            transform.localScale = Vector3.zero;
            transform.SetPositionAndRotation(new Vector3(parent.position.x + 10, parent.position.y, zpos), Quaternion.identity);
            LeanTween.scale(gameObject, InitialScale, .1f);
            LeanTween.moveLocalX(gameObject, parent.position.x, .1f);
            transform.SetParent(parent);
            Activate();
        }

        public virtual void FlipAndDeactivate()
        {
            LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 180, .2f).setLoopCount(0);
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + UnityEngine.Random.Range(20, 30), .75f).setOnComplete(Deactivate);
        }

        public override void Deactivate()
        {
            try
            {
                LeanTween.cancel(gameObject);
            }
            catch (Exception) { }
            transform.localScale = Vector3.zero;
            base.Deactivate();
        }

        public void SetTexture(Texture cubeTexture)
        {
            if (m_Renderer == null)
                GetRenderer();
            if (cubeTexture != null && m_Renderer != null)
            {
                m_Renderer.material.SetTexture("_MainTex", cubeTexture);
            }
        }

        private void GetRenderer()
        {
            if (m_Renderer != null)
                m_Renderer = gameObject.GetComponent<MeshRenderer>();
        }

        public void SetBaseCubeType(CubeType cubeType)
        {
            if (m_Renderer != null)
                m_BaseCubeType = cubeType;
        }

        public void SetColor(Color color)
        {
            if (m_Renderer != null)
                m_Renderer.material.SetColor("_CubeColor", color);
        }

        public virtual void PlayBounceSound()
        {
            if (!GameData.Instance.IsPlayerScaled)
                AudioManager.Instance.PlayEffect(m_BounceSoundNormal);
            else
                AudioManager.Instance.PlayEffect(m_BounceSoundHeavy);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public CubeType GetBaseCubeType()
        {
            return m_BaseCubeType;
        }

        public float GetEffectDuration()
        {
            return EffectDuration;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public RectTransform GetRectTransform()
        {
            return m_RectTransform;
        }
    }
}