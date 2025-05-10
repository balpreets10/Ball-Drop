using BallDrop.Audio;
using BallDrop.Interfaces;
using System;
using UnityEngine;

namespace BallDrop.Base
{
    public class BaseCube : MonoBehaviour, ICube
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

        public virtual void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            InitialScale = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                //if (transform.parent.GetComponent<RowBase>() != null)
                //    transform.parent.GetComponent<RowBase>().RemoveFromCubes(this);
                //transform.parent = ObjectPool.Instance.PooledObjectsHolder;
                //GameData.Instance.playerGameData.AddCubeData(GetBaseCubeType());
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
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(true);
        }

        public virtual void FlipAndDeactivate()
        {
            LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 180, .2f).setLoopCount(0);
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + UnityEngine.Random.Range(20, 30), .75f).setOnComplete(Deactivate);
        }

        public virtual void Deactivate()
        {
            try
            {
                LeanTween.cancel(gameObject);
            }
            catch (Exception) { }
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void SetTexture(Texture cubeTexture)
        {
            if (m_Renderer == null)
                GetRenderer();
            if (cubeTexture != null)
            {
                m_Renderer.material.SetTexture("_MainTex", cubeTexture);
            }
        }

        private void GetRenderer()
        {
            m_Renderer = gameObject.GetComponent<MeshRenderer>();
        }

        public void SetBaseCubeType(CubeType cubeType)
        {
            m_BaseCubeType = cubeType;
        }

        public void SetColor(Color color)
        {
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