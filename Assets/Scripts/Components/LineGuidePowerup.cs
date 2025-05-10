using System.Collections;
using UnityEngine;
namespace BallDrop
{
    public class LineGuidePowerup : MonoBehaviour
    {
        private LineRenderer lr;
        bool PowerEnabled = false;
        Vector3 endPosition = Vector3.zero;
        Vector3 Startposition = Vector3.zero;

        void Awake()
        {
            lr = GetComponentInChildren<LineRenderer>();
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, Vector3.zero);
        }

        private void OnEnable()
        {
            MyEventManager.Instance.OnLandedOnXCube.AddListener(OnLandedOnXCube);
            MyEventManager.Instance.OnLineGuideCollected.AddListener(OnLineGuideCollected);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.OnLandedOnXCube.RemoveListener(OnLandedOnXCube);
                MyEventManager.Instance.OnLineGuideCollected.RemoveListener(OnLineGuideCollected);


            }
        }

        private void OnLineGuideCollected(float duration)
        {
            StartCoroutine(DisableLineGuide(duration));
        }

        private IEnumerator DisableLineGuide(float duration)
        {
            PowerEnabled = true;
            yield return new WaitForSeconds(duration);
            PowerEnabled = false;
            lr.SetPosition(1, Vector3.zero);
        }

        private void OnLandedOnXCube(float duration, float ScaleFactor)
        {
            lr.startWidth = lr.endWidth = ScaleFactor;
            StartCoroutine(NormalizeWidth(duration));
        }

        private IEnumerator NormalizeWidth(float duration)
        {
            yield return new WaitForSeconds(duration);
            lr.startWidth = lr.endWidth = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (PowerEnabled)
            {
                lr.SetPosition(0, Startposition);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 20f, LayerMask.GetMask("Row")))
                {
                    if (!hit.collider.CompareTag("Row"))
                    {
                        Debug.Log("Hit pos - " + hit.point);
                        // endPosition.y = hit.point.y;
                        lr.SetPosition(1, hit.point);

                    }


                }
            }
        }
    }
}