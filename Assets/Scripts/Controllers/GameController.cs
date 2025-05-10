using BallDrop.Base;
using BallDrop.Interfaces;
using BallDrop.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallDrop
{
    public class GameController : MonoBehaviour
    {
        private List<Vector3> Axes = new List<Vector3>();
        private void OnEnable()
        {
            MyEventManager.OnRowsSpawned.AddListener(OnRowsSpawned);
            MyEventManager.OnPlayerActivated.AddListener(OnPlayerActivated);
        }

        private void Start()
        {
            Axes.Add(Vector3.right);
            Axes.Add(Vector3.up);
            Axes.Add(Vector3.forward);
            Axes.Add(-Vector3.right);
            Axes.Add(-Vector3.up);
            Axes.Add(-Vector3.forward);
            Audio.AudioManager.Instance.PlayBgMusic();
        }

        private void OnDisable()
        {
                MyEventManager.OnRowsSpawned.RemoveListener(OnRowsSpawned);
                MyEventManager.OnPlayerActivated.RemoveListener(OnPlayerActivated);
        }

        private void OnPlayerActivated()
        {
            GameData.Instance.IncreaseRowSpeed();
            StartCoroutine(SpawnRandomObjects());
        }

        private void OnRowsSpawned()
        {
            ObjectPool.Instance.GetPlayer().GetComponent<IPlayer>().ActivateAndSetPosition();
        }

        private IEnumerator SpawnRandomObjects()
        {
            GameObject randomObject = ObjectPool.Instance.GetRandomObject();
            if (randomObject != null)
            {
                randomObject.SetActive(true);
                LeanTween.rotateAroundLocal(randomObject, Axes[UnityEngine.Random.Range(0, Axes.Count)], 360, 2f);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(5, 15));
        }
    }
}