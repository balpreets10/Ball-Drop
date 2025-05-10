using UnityEngine;

namespace BallDrop
{

    public class UnlockableItems : MonoBehaviour
    {
        public GameObject SelectionIcon, LockIcon, UnlockedIcon;
        public GameObject IconPanel;

        public int index;
        public ItemType MyItemType;
        private bool IsUnlocked = false;

        private Vector2 highlightPosition = new Vector2(0, 20), nonHighlightPosition = new Vector2(0, -50);

        private void Awake()
        {
            LockIcon.SetActive(false);
            SelectionIcon.SetActive(false);
            UnlockedIcon.SetActive(false);
        }

        private void OnEnable()
        {
            MyEventManager.UpdateScrollItems.AddListener(UpdateScrollItems);
            MyEventManager.UpdateLockStatus.AddListener(SetLocked);
        }

        private void OnDisable()
        {
                MyEventManager.UpdateScrollItems.RemoveListener(UpdateScrollItems);
                MyEventManager.UpdateLockStatus.RemoveListener(SetLocked);
        }

        private void UpdateScrollItems(int index, ItemType item)
        {
            if (this.index == index && item == MyItemType)
            {
                LeanTween.scale(gameObject, Vector3.one * 2.5f, .1f).setOnComplete(SetIconPosition).setOnCompleteParam(highlightPosition);
            }
            else if (this.index != index && item == MyItemType)
            {
                LeanTween.scale(gameObject, Vector3.one * 1.4f, .1f).setOnComplete(SetIconPosition).setOnCompleteParam(nonHighlightPosition);
            }
        }

        private void SetIconPosition(object position)
        {
            IconPanel.GetComponent<RectTransform>().anchoredPosition = (Vector2)(position);

        }

        public void SetLocked(int index, ItemType item, LockStatus lockStatus)
        {
            if (this.index == index && item == MyItemType)
            {
                SetIcon(lockStatus);
            }
        }

        public void SetIcon(LockStatus status)
        {
            switch (status)
            {
                case LockStatus.Locked:
                    LockIcon.SetActive(true);
                    UnlockedIcon.SetActive(false);
                    SelectionIcon.SetActive(false);
                    transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 0.4f;
                    break;
                case LockStatus.Unlocked:
                    LockIcon.SetActive(false);
                    UnlockedIcon.SetActive(true);
                    SelectionIcon.SetActive(false);
                    transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
                    break;
                case LockStatus.Selected:
                    LockIcon.SetActive(false);
                    UnlockedIcon.SetActive(false);
                    SelectionIcon.SetActive(true);
                    transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1f;
                    break;
            }
        }
    }
}