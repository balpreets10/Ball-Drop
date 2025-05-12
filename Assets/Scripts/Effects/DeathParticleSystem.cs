using BallDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleSystem : MonoBehaviour
{
    private GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }

    private void OnEnable()
    {
        MyEventManager.QuitGame.AddListener(OnQuitGame);
    }

    private void OnDisable()
    {
        MyEventManager.QuitGame.RemoveListener(OnQuitGame);
    }

    private void OnQuitGame()
    {
        if (parent.activeInHierarchy)
        {
            parent.SetActive(false);
            parent.transform.SetParent(ObjectPool.Instance.PooledObjectsHolder);
        }
    }

    public void OnParticleSystemStopped()
    {
        parent.SetActive(false);
        parent.transform.SetParent(ObjectPool.Instance.PooledObjectsHolder);
        MySceneManager.Instance.LoadScene(Scenes.GameEnd);
    }
}