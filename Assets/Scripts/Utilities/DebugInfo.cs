using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using TMPro;

namespace BallDrop.Utilities
{
    public class DebugInfo : MonoBehaviour
    {
        public Color[] _Colors = new Color[] { Color.white, Color.red, Color.green, Color.blue };
        public TextMeshProUGUI _Text = null;
        public int _SkipFrames = 5;

        private static bool mCreated = false;
        private int mIndex = 0;
        private int mFrameCount = 0;

#if UNITY_IOS || UNITY_EDITOR
        private float mMBSize = 1f / (1024f * 1024f);
#endif

        private void Start()
        {
            if (!mCreated)
            {
                mCreated = true;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
#if RELEASE || LIVE_BUILD || DEMO_BUILD
        Show(false);
#endif
        }

        private void Update()
        {
            mFrameCount++;
            if (mFrameCount >= _SkipFrames)
            {
                mFrameCount = 0;
                StringBuilder data = new StringBuilder();
                //data.AppendLine("FPS : " + QualityManager.pFrameRate);

#if UNITY_IOS || UNITY_EDITOR
                data.AppendLine("Free : " + (MobileUtilities.GetFreeMemory() * mMBSize) + " \nInUse : " + (MobileUtilities.GetMemoryInUse() * mMBSize));
                data.AppendLine("CPU: " + MobileUtilities.GetDeviceCPUUsage());
#endif
                //data.AppendLine("QLT : " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
                //if (!PlatformUtilities.IsLocalBuild())
                //    data.AppendLine("BL : " + UiPrefetchManager.GetStatus());
                if (_Text != null)
                    _Text.text = data.ToString();
            }
        }

        public void OnDrag(BaseEventData eventData)
        {
            _Text.transform.parent.position = Input.mousePosition;
        }

        public void ChangeColor()
        {
            mIndex++;
            if (mIndex >= _Colors.Length)
                mIndex = 0;
            _Text.color = _Colors[mIndex];
        }

        public void Show(bool show)
        {
            _Text.transform.parent.gameObject.SetActive(show);
        }
    }
}