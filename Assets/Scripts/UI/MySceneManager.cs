using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BallDrop
{
    public enum Scenes
    {
        Splash,
        Menu,
        Game,
        GameEnd
    }

    public class MySceneManager : SingletonMonoBehaviour<MySceneManager>
    {
        CanvasGroup canvasGroup;
        Scenes CurrentScene = Scenes.Splash;
        Scenes previousScene;

        private void Awake()
        {
            canvasGroup = GetComponentInChildren<Canvas>().GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            HideLoadingCanvas();
        }

        public void ShowLoadingCanvas()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
        }

        public void HideLoadingCanvas()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
        }

        public Scenes GetPrevious()
        {
            return previousScene;
        }

        public void LoadScene(Scenes scene, bool async = true)
        {
            previousScene = CurrentScene;
            CurrentScene = scene;
            if (async)
                StartCoroutine(LoadNewScene(scene));
            else
                SceneManager.LoadScene(scene.ToString());
        }

        private IEnumerator LoadNewScene(Scenes scene)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(scene.ToString());
            ShowLoadingCanvas();

            while (!async.isDone)
            {
                yield return null;
            }
        }
    }

}