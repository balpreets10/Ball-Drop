using System;
using BallDrop.Interfaces;
using UnityEngine;

namespace BallDrop
{
    public class Bounce : MonoBehaviour
    {
        private float GoingDownFactor = 1;
        private bool completed = false;
        private ICube collisionCube = null;
        private PlayerBase myPlayerBase;

        public PlayerState playerState;
        public LeanTweenType bounceEase;

        #region --------------------LISTENERS-------------------------
        private void OnEnable()
        {
            MyEventManager.OnPlayerActivated.AddListener(ResetData);
            MyEventManager.OnCompletedRevivalAd.AddListener(OnCompletedRevivalAd);
        }

        private void OnDisable()
        {
                MyEventManager.OnPlayerActivated.RemoveListener(ResetData);
                MyEventManager.OnCompletedRevivalAd.RemoveListener(OnCompletedRevivalAd);
        }
        #endregion

        private void Start()
        {
            myPlayerBase = GetComponent<PlayerBase>();
            playerState = PlayerState.GoingDown;
        }


        private void Update()
        {
            if (playerState == PlayerState.GoingDown)
            {
                BallDown();
                GoingDownFactor += 0.04f;
            }
        }

        private void ResetData()
        {
            completed = false;
            GoingDownFactor = 1;
            playerState = PlayerState.GoingDown;
        }


        private void BallDown()
        {
            transform.Translate(Vector2.down * Time.deltaTime * GameData.Instance.BallBounceSpeed * GoingDownFactor);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(CubeType.Normal.ToString()) || collision.gameObject.CompareTag(CubeType.Moving.ToString()))
            {
                SquishEffect(collision.gameObject);
                ChangeStateToUp();
                collisionCube = collision.gameObject.GetComponent<ICube>();
                collisionCube.PlayBounceSound();
            }
            else if (collision.gameObject.CompareTag(CubeType.Faded.ToString()))
            {
                ObjectPool.Instance.GetBreakParticle().Activate(transform.position, ColorData.Instance.GetPrimaryColor());
                collisionCube = collision.gameObject.GetComponent<ICube>();
                collision.gameObject.GetComponent<CubeFaded>().Disappear();
                collisionCube.PlayBounceSound();
            }
            else if (collision.gameObject.CompareTag(CubeType.Enemy.ToString()) || collision.gameObject.CompareTag(CubeType.Spike.ToString()))
            {
                if (myPlayerBase.IsShieldActive)
                {
                    SquishEffect(collision.gameObject);
                    ChangeStateToUp();
                    collisionCube = collision.gameObject.GetComponent<ICube>();
                    collisionCube.PlayBounceSound();
                }
                else
                {
                    if (GameData.Instance.enableDeath)
                        ReviveOnce();
                }
            }
            else if (collision.gameObject.CompareTag(CubeType.X.ToString()))
            {
                ChangeStateToUp();
                collisionCube = collision.gameObject.GetComponent<ICube>();
                collisionCube.PlayBounceSound();
                if (!myPlayerBase.IsShieldActive)
                {
                    myPlayerBase.IncreaseScale(collisionCube.GetEffectDuration());
                }
            }
            else if (collision.gameObject.CompareTag(CubeType.Reverse.ToString()))
            {
                ChangeStateToUp();
                SquishEffect(collision.gameObject);
                collisionCube = collision.gameObject.GetComponent<ICube>();
                collisionCube.PlayBounceSound();
                if (!myPlayerBase.IsShieldActive && GameData.Instance.enableReverse)
                {
                    myPlayerBase.SwitchMoveDirection(collisionCube.GetEffectDuration());
                }
            }
            else if (collision.gameObject.CompareTag(CubeType.Landing.ToString()))
            {
                ChangeStateToUp();
                collisionCube = collision.gameObject.GetComponent<ICube>();
                collisionCube.PlayBounceSound();
                if (!completed)
                {
                    LevelComplete();
                    completed = true;
                }
            }
        }

        private void BallUp()
        {
            GoingDownFactor = 1;
            float time = GameData.Instance.BallBounceDistance / GameData.Instance.BallBounceSpeed;
            LeanTween.moveLocalY(gameObject, transform.localPosition.y + GameData.Instance.BallBounceDistance, time).setEase(bounceEase).setOnComplete(ChangeStateToDown);
        }

        public void ChangeStateToDown()
        {
            playerState = PlayerState.GoingDown;
        }

        private void ChangeStateToUp()
        {
            playerState = PlayerState.GoingUp;
            BallUp();
            GameData.Instance.ResetMultiplier();
        }

        private void SquishEffect(GameObject cube)
        {
            ObjectPool.Instance.GetSplatterEffect().ActivateAndSetParent(myPlayerBase.m_Renderer.transform.position, cube.transform);
            LeanTween.scale(gameObject, new Vector3(transform.localScale.x, transform.localScale.y - 0.2f, transform.localScale.z + 0.2f), 0.05f).setOnComplete(RevertToOriginalState);
        }

        private void RevertToOriginalState()
        {
            if (!GameData.Instance.IsPlayerScaled)
                LeanTween.scale(gameObject, GameData.Instance.initialScale, 0.1f);
            else
                LeanTween.scale(gameObject, GameData.Instance.initialScale * 1.5f, 0.1f);
        }

        public void ReviveOnce()
        {
            playerState = PlayerState.GoingUp;
            if (UnityAdManager.Instance.CheckLifeRewardReady())
            {
                GameData.Instance.cameraController.DisableCollider();
                MyEventManager.ReviveOption.Dispatch();
            }
            else
            {
                myPlayerBase.Deactivate();
                MyEventManager.OnPlayerDeath.Dispatch();
            }
        }

        private void OnCompletedRevivalAd()
        {
            myPlayerBase.TriggeredOnce = false;
            if (transform.position.y < GameData.Instance.cameraController.MainCamera.transform.position.y - 14)
            {
                GameData.Instance.cameraController.MoveDown();
            }
            else if (transform.position.y > GameData.Instance.cameraController.MainCamera.transform.position.y - 2)
            {
                GameData.Instance.cameraController.MoveUp();
            }
            else
            {
                GameData.Instance.cameraController.EnableCollider();
            }
            MyEventManager.SetPlayerPosAfterRevival.Dispatch(gameObject);
        }

        public void LevelComplete()
        {
            MyEventManager.OnLevelCompleted.Dispatch();
            myPlayerBase.Invoke("Reset", 2f);
        }

        #region ContextMenus
        [ContextMenu("GetViewportPoint")]
        public void GetViewPortPoint()
        {
            Debug.Log(Camera.main.WorldToViewportPoint(transform.position));
        }
        #endregion

    }
}