using System;
using UnityEngine;
using UnityEngine.UI;
namespace BallDrop
{
    public class Fillable : MonoBehaviour
    {
        public Image FillImage;
        private FillablePanel fillablePanel;

        private void OnEnable()
        {
            MyEventManager.EndGame.AddListener(EndGame);
        }

        private void OnDisable()
        {
                MyEventManager.EndGame.RemoveListener(EndGame);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public virtual void Activate(float duration)
        {
            ShowPowerup(duration);
        }

        private void ShowPowerup(float duration)
        {
            gameObject.SetActive(true);
            FillImage.fillAmount = 0;
            LeanTween.value(gameObject, 0, 1, duration).setOnUpdate(OnFillAmountChange).setEase(LeanTweenType.linear).setOnComplete(Deactivate);
        }

        private void OnFillAmountChange(float fillValue)
        {
            FillImage.fillAmount = fillValue;
        }

        public virtual void Deactivate()
        {
            DeactivateFillable();
        }

        private void DeactivateFillable()
        {
            LeanTween.cancel(gameObject);
            FillImage.fillAmount = 0;
            gameObject.SetActive(false);
            fillablePanel.OnFillableDeactivated();
        }

        public void ResetFillable()
        {
            FillImage.fillAmount = 0;
            gameObject.SetActive(false);
        }

        public void SetFillablePanel(FillablePanel panel)
        {
            fillablePanel = panel;
        }

        public virtual void EndGame()
        {
            LeanTween.cancel(gameObject);
        }

    }
}