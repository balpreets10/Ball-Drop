using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class BackgroundManager : SingletonMonoBehaviour<BackgroundManager>
    {
        [SerializeField]
        private List<Texture> textures;

        private int index = 0;

        private void OnEnable()
        {
            MyEventManager.Instance.ChangeTexture.AddListener(ChangeTexture);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.ChangeTexture.RemoveListener(ChangeTexture);
            }
        }

        public void ChangeTexture()
        {
            index++;
            if (index >= textures.Count)
                index = 0;
            MyEventManager.Instance.OnBackgroundUpdated.Dispatch(textures[index]);
        }
    }

}