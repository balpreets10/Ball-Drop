using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BallDrop
{
    public class UICanvas : MonoBehaviour

    {
        public Canvas canvas;
        public GraphicRaycaster graphicRaycaster;
        void Awake()
        {
            canvas = GetComponent<Canvas>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);
        }

        private void OnEnable()
        {
           // MyEventManager.Instance.ShowMenu.AddListener(ShowMenu);
            //MyEventManager.Instance.EndGame.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
               // MyEventManager.Instance.ShowMenu.RemoveListener(ShowMenu);
                // MyEventManager.Instance.EndGame.RemoveListener(OnGameEnd);

            }
        }

        private void OnGameEnd()
        {
            StartCoroutine(EnableCanvas(true, 0));
        }

        //private void ShowMenu(MenuType type, float delay)
        //{
        //    if (type == MenuType.Game)
        //    {
        //        StartCoroutine(EnableCanvas(false, 0f));
        //    }
        //    else
        //    {
        //        StartCoroutine(EnableCanvas(true, delay));
        //    }
        //}

        IEnumerator EnableCanvas(bool enable, float delay)
        {
            yield return new WaitForSeconds(delay + 0.2f);
            canvas.enabled = graphicRaycaster.enabled = enable;
        }



    }
}