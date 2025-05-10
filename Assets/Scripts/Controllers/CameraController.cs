using System.Collections;
using UnityEngine;

namespace BallDrop
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 InitialPosition = new Vector3();

        public Transform DeathParticlePosition;
        public GameObject BackgroundParticles;
        public Camera MainCamera;

        private Coroutine coroutine;
        private BoxCollider CameraCollider;

        private void Start()
        {
            MainCamera = Camera.main;
            GameData.Instance.cameraController = this;
            ObjectPool.Instance.SetTrails();
            InitialPosition = transform.position;
            BackgroundParticles.SetActive(true);
            CameraCollider = MainCamera.GetComponent<BoxCollider>();
            Vector2 aspectRatio = AspectRatio.GetAspectRatio(Screen.width, Screen.height, true);
            if (aspectRatio.x == 9 && aspectRatio.y == 19)
            {
                MainCamera.orthographicSize = 9;
            }
            else if (aspectRatio.x == 9 && aspectRatio.y == 16)
            {
                MainCamera.orthographicSize = 8;
            }
            else if (aspectRatio.x == 16 && aspectRatio.y == 9)
            {
                MainCamera.orthographicSize = 7;
            }
            else if (aspectRatio.x == 3 && aspectRatio.y == 4)
            {
                MainCamera.orthographicSize = 7;
            }
            else if (aspectRatio.x == 5 && aspectRatio.y == 8)
            {
                MainCamera.orthographicSize = 8;
            }
            else if (aspectRatio.x == 6 && aspectRatio.y == 13)
            {
                MainCamera.orthographicSize = 9;
            }
            MainCamera.orthographicSize += 2;
            GameObject[] whatever = FindObjectsOfType(typeof(QuadController)) as GameObject[];
            if (whatever != null && whatever.Length > 0)
                foreach (GameObject quad in whatever)
                {
                    Debug.Log("Name - " + quad.name);
                }
            SetupCameraCollider();
        }

        private void SetupCameraCollider()
        {
            CameraCollider.size = new Vector3(25, MainCamera.orthographicSize * 2, MainCamera.farClipPlane);
        }

        private void OnEnable()
        {
            MyEventManager.Instance.OnRowsSpawned.AddListener(StartGame);
            MyEventManager.Instance.OnLevelCompleted.AddListener(CenterLandingRow);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.OnRowsSpawned.RemoveListener(StartGame);
                MyEventManager.Instance.OnLevelCompleted.RemoveListener(CenterLandingRow);
            }

        }

        private void StartGame()
        {
            coroutine = StartCoroutine(MoveCamera());
        }

        private IEnumerator MoveCamera()
        {
            while (true)
            {
                transform.Translate(Vector2.down * Time.deltaTime * GameData.Instance.CurrentRowMovementSpeed);
                yield return null;
            }
        }

        public void MoveDown()
        {
            LeanTween.moveY(gameObject, transform.position.y - GameData.Instance.SpaceBetweenRows * 2.5f, 0.7f).setOnComplete(EnableCollider);
        }

        public void MoveUp()
        {
            LeanTween.moveY(gameObject, transform.position.y + GameData.Instance.SpaceBetweenRows * 1.5f, 0.5f).setOnComplete(EnableCollider);
        }

        private void CenterLandingRow()
        {
            EnableCollider();
            StopCoroutine(coroutine);
            LeanTween.moveY(gameObject, RowSpawner.CurrentRow.transform.position.y, 0.7f);
        }

        public void DisableCollider()
        {
            CameraCollider.enabled = false;
        }

        public void EnableCollider()
        {
            if (!CameraCollider.enabled)
                CameraCollider.enabled = true;
        }
    }
}