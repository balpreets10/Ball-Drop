using UnityEngine;
namespace BallDrop
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Popup : MonoBehaviour
    {

        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;
        private Vector2 ScreenSize;

        public enum Direction
        {
            ToRight,
            ToLeft,
            ToUp,
            ToDown,
        }
        public enum PopupState
        {
            Visible,
            Hidden
        }


        public Direction ShowDirection = Direction.ToLeft;
        public Direction HideDirection = Direction.ToRight;
        public PopupState popupState = PopupState.Hidden;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            ScreenSize = new Vector2(Screen.width, Screen.height);
            //Debug.Log("Screen Size - " + Screen.width + "x" + Screen.height);
            ScreenSize.x += ScreenSize.x;
            ScreenSize.y += ScreenSize.y;
        }

        public virtual void ShowPopup()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1;

            switch (ShowDirection)
            {
                case Direction.ToLeft:
                    transform.localPosition = new Vector3(ScreenSize.x, 0);
                    break;
                case Direction.ToRight:
                    transform.localPosition = new Vector3(-ScreenSize.x, 0);
                    break;
                case Direction.ToUp:
                    transform.localPosition = new Vector3(0, -ScreenSize.y);
                    break;
                case Direction.ToDown:
                    transform.localPosition = new Vector3(0, ScreenSize.y);
                    break;
            }
            LeanTween.moveLocal(gameObject, Vector3.zero, .35f).setEase(LeanTweenType.easeOutBack).setOnComplete(OnPopupShown);
        }

        public virtual void OnPopupShown()
        {
            canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
            popupState = PopupState.Visible;
        }

        public virtual void HidePopup(bool instant = false)
        {
            Vector3 destination = Vector3.one;
            switch (HideDirection)
            {
                case Direction.ToLeft:
                    destination = new Vector3(-ScreenSize.x, 0, 0);
                    break;
                case Direction.ToRight:
                    destination = new Vector3(ScreenSize.x, 0, 0);
                    break;
                case Direction.ToUp:
                    destination = new Vector3(0, ScreenSize.y, 0);
                    break;
                case Direction.ToDown:
                    destination = new Vector3(0, -ScreenSize.y, 0);
                    break;
            }
            if (instant)
            {
                transform.localPosition = destination;
            }
            canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            LeanTween.moveLocal(gameObject, destination, .2f).setEase(LeanTweenType.easeInBack).setOnComplete(OnPopupHidden);
        }

        private void OnPopupHidden()
        {
            popupState = PopupState.Hidden;
        }
    }
}