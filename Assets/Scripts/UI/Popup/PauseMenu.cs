using BallDrop.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallDrop
{
    public class PauseMenu : Popup
    {
        private void OnEnable()
        {
            MyEventManager.Instance.PauseGame.AddListener(OnPause);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.PauseGame.RemoveListener(OnPause);
            }
        }

        private void OnPause()
        {
            ShowPopup();            
        }

        public override void OnPopupShown()
        {
            base.OnPopupShown();
            UpdateTimeScale(0);
        }

        public void ResumeGame()
        {
            UpdateTimeScale(1);
            HidePopup(true);
            AudioManager.Instance.Unpause();
        }

        private void UpdateTimeScale(float value)
        {
            Time.timeScale = value;
        }

        public void QuitGame()
        {
            UpdateTimeScale(1);
            MyEventManager.Instance.EndGame.Dispatch();
            MyEventManager.Instance.QuitGame.Dispatch();
            HidePopup(true);
            MySceneManager.Instance.LoadScene(Scenes.Menu);
        }

    }
}