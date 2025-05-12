using System.Collections;
using UnityEngine;

namespace BallDrop
{
    public class LineGuidePowerup : GameComponent
    {
        private LineRenderer lineRenderer;
        private bool PowerEnabled = false;
        private Vector3 endPosition = Vector3.zero;
        private Vector3 Startposition = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            if (lineRenderer == null)
                lineRenderer = GetComponentInChildren<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }

        private void OnEnable()
        {
            MyEventManager.Game.OnLandedOnXCube.AddListener(OnLandedOnXCube);
            MyEventManager.Game.Powerups.OnLineGuideCollected.AddListener(OnLineGuideCollected);
        }

        private void OnDisable()
        {
            MyEventManager.Game.OnLandedOnXCube.RemoveListener(OnLandedOnXCube);
            MyEventManager.Game.Powerups.OnLineGuideCollected.RemoveListener(OnLineGuideCollected);
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
            if (lineRenderer != null)
                lineRenderer.SetPosition(1, Vector3.zero);
        }

        private void OnLandedOnXCube(float duration, float ScaleFactor)
        {
            if (lineRenderer != null)
                lineRenderer.startWidth = lineRenderer.endWidth = ScaleFactor;
            StartCoroutine(NormalizeWidth(duration));
        }

        private IEnumerator NormalizeWidth(float duration)
        {
            yield return new WaitForSeconds(duration);
            if (lineRenderer != null)
                lineRenderer.startWidth = lineRenderer.endWidth = 1.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (PowerEnabled)
            {
                if (lineRenderer != null)
                    lineRenderer.SetPosition(0, Startposition);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 20f, LayerMask.GetMask("Row")))
                {
                    if (!hit.collider.CompareTag("Row"))
                    {
                        Debug.Log("Hit pos - " + hit.point);
                        if (lineRenderer != null)
                            lineRenderer.SetPosition(1, hit.point);
                    }
                }
            }
        }
    }
}