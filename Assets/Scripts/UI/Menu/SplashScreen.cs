using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BallDrop
{
    public class SplashScreen : PrimaryMenu
    {
        public Image LoadingSlider;
        public Image Title;
        public TextMeshProUGUI percentage;
        public MeshRenderer m_Renderer;

        private bool ObjectsInstantiated = false;
        private bool SliderFilled = false;

        private void OnEnable()
        {
            MyEventManager.OnObjectsInstantiated.AddListener(OnObjectInstantiated);
        }

        private void OnDisable()
        {
                MyEventManager.OnObjectsInstantiated.RemoveListener(OnObjectInstantiated);
        }

        public override void Start()
        {
            base.Start();
            ObjectsInstantiated = false;
            SliderFilled = false;
            percentage.text = "0%";
            StartCoroutine(ObjectPool.Instance.InstantiateObjects());
            LoadingSlider.fillAmount = 0;
            LeanTween.value(0, .5f, 3f).setOnUpdate(UpdateStripHeight).setOnComplete(OnSliderFilled);
            LeanTween.moveLocalX(m_Renderer.gameObject, -400, 1f).setOnUpdate(RotateSphere).setLoopPingPong();
        }

        private void RotateSphere(float val)
        {
            m_Renderer.gameObject.transform.rotation = Quaternion.Euler(new Vector3(m_Renderer.gameObject.transform.rotation.eulerAngles.x + 6,
                m_Renderer.gameObject.transform.rotation.eulerAngles.y, m_Renderer.gameObject.transform.rotation.eulerAngles.z + 6));
        }

        private void LoadMainMenu()
        {
            if (SliderFilled && ObjectsInstantiated)
            {
                percentage.text = "Welcome";
                LeanTween.cancel(m_Renderer.gameObject);
                m_Renderer.gameObject.SetActive(false);
                MyEventManager.Reveal.Dispatch();
                GameData.Instance.levelData.Level = PreferenceManager.Instance.GetIntPref(PrefKey.PlayerLevel, 1);
                MySceneManager.Instance.LoadScene(Scenes.Menu, false);

            }
        }

        private void OnObjectInstantiated()
        {
            ObjectsInstantiated = true;
            LoadMainMenu();
        }

        void OnSliderFilled()
        {
            SliderFilled = true;
            percentage.text = "Loading...";
            LoadMainMenu();
        }

        private void OnSliderValueChanged(float value)
        {
            LoadingSlider.fillAmount = value;
            percentage.text = (int)(value * 100) + "%";
        }

        private void UpdateStripHeight(float value)
        {
            m_Renderer.material.SetFloat("_StripHeight", value);
            percentage.text = (int)(value * 200) + "%";
        }

    }
}