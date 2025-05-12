using BallDrop.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace BallDrop
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerBase : MonoBehaviour, IPlayer
    {
        private LTDescr tweenRotate, tweenStrip;

        private enum ChangeDirection
        {
            Positive,
            Negative
        }

        [SerializeField] protected MeshRenderer m_Renderer;
        protected Rigidbody m_Rigidbody;
        [SerializeField] protected PlayerShield Shield;
        [SerializeField] protected bool isShieldActive = false;
        [SerializeField] public bool TriggeredOnce = false;
        private IPowerup currentPowerup = null;
        private WaitForSeconds delay = new WaitForSeconds(0.035f);

        //Swipe
        private Vector2 m_FirstPressPos, m_SecondPressPos;

        private Vector2 m_CurrentSwipe;
        private Vector3 m_MoveDirection = new Vector3(0, 0, 1);
        private int SwitchDirection = 1;
        public bool IsDirectionReversed = false;

        //Trail
        private Vector3 trailPosition;

        private readonly float m_MaxTrailStartWidth = .665f;
        private readonly float m_MinTrailStartWidth = .55f;

        [SerializeField]
        private readonly float m_MaxTrailStartWidthScaled;

        [SerializeField]
        private readonly float m_MinTrailStartWidthScaled;

        private float ScaleFactor = 1.5f;
        private Coroutine trailCoroutine = null;
        private TrailScaleData scaleData;

        #region LIFE CYCLE

        private void Start()
        {
            if (m_Rigidbody == null)
                m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            MyEventManager.Game.Rows.OnRowPassed.AddListener(OnRowPassed);
            MyEventManager.QuitGame.AddListener(Reset);
            MyEventManager.OnCancelledRevive.AddListener(OnCancelledRevive);

            UpdateBallColor();
            SwitchDirection = 1;
        }

        private void OnDisable()
        {
            MyEventManager.OnCancelledRevive.RemoveListener(OnCancelledRevive);
            MyEventManager.Game.Rows.OnRowPassed.RemoveListener(OnRowPassed);
            MyEventManager.QuitGame.RemoveListener(Reset);
        }

        private void Update()
        {
            //m_Rigidbody.velocity = Vector3.zero;
            if (Application.platform == RuntimePlatform.Android)
                TouchSwipe();
            else
            {
                MouseSwipe();
            }
        }

        #endregion LIFE CYCLE

        #region TRIGGER

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GameStrings.PowerUp))
            {
                currentPowerup = other.gameObject.GetComponent<IPowerup>();
                if (currentPowerup != null)
                {
                    currentPowerup.PlayPowerupSound();
                    if (currentPowerup.GetPowerupType() == PowerupType.SlowDown)
                        OnSlowDownCollected(currentPowerup.GetPowerupDuration());
                    else if (currentPowerup.GetPowerupType() == PowerupType.Shield)
                        OnShieldCollected(currentPowerup.GetPowerupDuration());
                    else if (currentPowerup.GetPowerupType() == PowerupType.VerticalBeam)
                        OnProjectileCollected(currentPowerup.GetPowerupDuration());

                    currentPowerup.Deactivate();
                    currentPowerup = null;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameStrings.MainCamera))
            {
                if (GameData.Instance.enableDeath)
                {
                    if (!TriggeredOnce)
                    {
                        TriggeredOnce = true;
                        Bounce bounce = GetComponent<Bounce>();
                        if (bounce != null)
                            bounce.ReviveOnce();
                    }
                }
            }
        }

        #endregion TRIGGER

        private void OnCancelledRevive()
        {
            GameData.Instance.cameraController.EnableCollider();
            Deactivate();
            MyEventManager.Game.OnPlayerDeath.Dispatch();
        }

        private void UpdateBallColor()
        {
            if (GameData.Instance.gameMode == GameMode.Classic)
                tweenStrip = LeanTween.value(m_Renderer.gameObject, 1, 0, 1.2f).setOnUpdate(UpdateStripHeight);
            else
                tweenStrip = LeanTween.value(m_Renderer.gameObject, 1, .3f, .5f).setOnUpdate(UpdateStripHeight);
        }

        private void OnRowPassed()
        {
            if (GameData.Instance.gameMode == GameMode.Classic)
                UpdateStripHeight(GameData.Instance.levelData.GetLevelProgress() / 2);
        }

        private void OnSlowDownCollected(float duration)
        {
            GameData.Instance.CurrentTypes.Add(PowerupType.SlowDown);
            MyEventManager.Game.Powerups.OnSlowDownCollected.Dispatch(duration);
        }

        private void OnShieldCollected(float duration)
        {
            isShieldActive = true;
            ToggleShield(true, ColorData.Instance.GetPrimaryColor());
            GameData.Instance.CurrentTypes.Add(PowerupType.Shield);
            StartCoroutine(DisableShield(duration));
            MyEventManager.Game.Powerups.OnShieldCollected.Dispatch(duration);
        }

        private void ToggleShield(bool toggle, Color color)
        {
            if (Shield != null)
            {
                if (toggle)
                {
                    Shield.Activate(color);
                }
                else
                {
                    Shield.Deactivate();
                }
            }
        }

        private void OnProjectileCollected(float duration)
        {
            StartCoroutine(ActivateProjectiles(duration));
        }

        private IEnumerator ActivateProjectiles(float duration)
        {
            for (int i = 0; i < 3; i++)
            {
                ProjectileParticle projectile = ObjectPool.Instance.GetProjectile();
                projectile.Activate(transform.position);
                yield return new WaitForSeconds(duration);
            }
        }

        public Vector3 GetCurrentPosition()
        {
            if (m_Renderer != null)
                return m_Renderer.transform.position;
            else
                return Vector3.zero;
        }

        private void OnLineGuideCollected(float duration)
        {
            MyEventManager.Game.Powerups.OnLineGuideCollected.Dispatch(duration);
        }

        public bool IsShieldActive()
        {
            return isShieldActive;
        }

        public bool IsTriggeredOnce()
        {
            return TriggeredOnce;
        }

        private void MouseSwipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_FirstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                m_SecondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                MovePlayer();
                m_FirstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        private void TouchSwipe()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    m_FirstPressPos = touch.position;
                }
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Moved)
                {
                    m_SecondPressPos = touch.position;
                    MovePlayer();
                    m_FirstPressPos = touch.position;
                }
            }
        }

        public void MovePlayer()
        {
            //create vector from the two points
            m_CurrentSwipe = new Vector2(m_SecondPressPos.x - m_FirstPressPos.x, m_SecondPressPos.y - m_FirstPressPos.y);

            if (Mathf.Abs(m_CurrentSwipe.x) <= GameData.Instance.SwipeDetectionSensitivity)
            {
                return;
            }

            //normalize the 2d vector
            m_CurrentSwipe.Normalize();
            if (m_CurrentSwipe.x < 0)
            {
                transform.Translate(-m_MoveDirection * (GameData.Instance.PlayerMovementSensitivity * Time.deltaTime * SwitchDirection));
            }
            else if (m_CurrentSwipe.x > 0/* && (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)*/)
            {
                transform.Translate(m_MoveDirection * (GameData.Instance.PlayerMovementSensitivity * Time.deltaTime * SwitchDirection));
            }

            if (transform.position.z < -.4f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -.4f);
                return;
            }
            if (transform.position.z > 7.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 7.5f);
                return;
            }
        }

        public void IncreaseScale(float duration)
        {
            if (!GameData.Instance.IsPlayerScaled)
            {
                MyEventManager.Game.OnLandedOnXCube.Dispatch(duration, ScaleFactor);
                transform.localScale = GameData.Instance.initialScale * ScaleFactor;
                GameData.Instance.IsPlayerScaled = true;
                StartCoroutine(ReturnToNormalScale(duration));
            }
        }

        public void ResetScale()
        {
            transform.localScale = GameData.Instance.initialScale;
            GameData.Instance.IsPlayerScaled = false;
        }

        public void SwitchMoveDirection(float duration)
        {
            if (!IsDirectionReversed)
            {
                IsDirectionReversed = true;
                SwitchDirection = -1;
                MyEventManager.Game.OnLandedOnReverseCube.Dispatch(duration);
                StartCoroutine(ResetMoveDirection(duration));
            }
        }

        public IEnumerator ResetMoveDirection(float duration)
        {
            yield return new WaitForSeconds(duration);
            IsDirectionReversed = false;
            SwitchDirection = 1;
        }

        public void ActivateAndSetPosition()
        {
            if (m_Renderer == null)
                m_Renderer = GetComponent<MeshRenderer>();
            transform.SetPositionAndRotation(GameData.Instance.PlayerStartingPosition, Quaternion.identity);
            if (ColorData.Instance.GetPrimaryColor() != null)
            {
                UpdateMaterialColor(ColorData.Instance.GetPrimaryColor());
            }
            ResetScale();
            ToggleShield(false, Color.white);
            TriggeredOnce = false;
            gameObject.SetActive(true);
            tweenRotate = LeanTween.rotateAround(m_Renderer.gameObject, Vector3.one, 360, 1.5f).setLoopType(LeanTweenType.linear);
            trailCoroutine = StartCoroutine(CreateTrails());
            MyEventManager.Game.OnPlayerActivated.Dispatch();
        }

        private IEnumerator CreateTrails()
        {
            while (true)
            {
                TrailImage trailImage = ObjectPool.Instance.GetTrailImage();
                if (trailImage != null)
                {
                    trailPosition.x = transform.position.x - 0.5f;
                    trailPosition.y = transform.position.y - 0.4f;
                    trailPosition.z = transform.position.z + 0.18f;
                    trailImage.Activate(trailPosition);
                }
                yield return delay;
            }
        }

        public void Deactivate()
        {
            Reset();
            StartParticles();
        }

        public void Reset()
        {
            StopCoroutine(trailCoroutine);
            ObjectPool.Instance.DeactivateTrails();
            if (tweenRotate != null)
                LeanTween.cancel(tweenRotate.id);
            if (tweenStrip != null)
                LeanTween.cancel(tweenStrip.id);

            gameObject.SetActive(false);
            gameObject.transform.localScale = GameData.Instance.initialScale;
        }

        private void StartParticles()
        {
            GameObject go = ObjectPool.Instance.GetDeathParticle();
            go.SetActive(true);
            go.transform.SetParent(GameData.Instance.cameraController.DeathParticlePosition.parent);
            go.transform.SetPositionAndRotation(GameData.Instance.cameraController.DeathParticlePosition.position, Quaternion.identity);
            ParticleSystem particleSystem = go.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
                particleSystem.Play();
        }

        private IEnumerator ReturnToNormalScale(float duration)
        {
            yield return new WaitForSeconds(duration);
            ResetScale();
        }

        private void UpdateStripHeight(float value)
        {
            if (m_Renderer != null)
                m_Renderer.material.SetFloat("_StripHeight", value);
        }

        private void UpdateMaterialColor(Color color)
        {
            if (m_Renderer != null)
                m_Renderer.material.SetColor("_StripColor", color);
        }

        private IEnumerator DisableShield(float duration)
        {
            yield return new WaitForSeconds(duration);
            ToggleShield(false, Color.white);
            isShieldActive = false;
            GameData.Instance.CurrentTypes.Remove(PowerupType.Shield);
        }
    }

    internal class TrailScaleData
    {
        public Vector3 StartScale;
        public Vector3 FinalScale;
    }
}