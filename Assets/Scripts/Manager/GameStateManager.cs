using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallDrop
{
    public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
    {
        //[SerializeField]
        //private GameState m_CurrentState;

        //private void Start()
        //{
        //}

        //public void UpdateState(GameState gameState)
        //{
        //    StartCoroutine(UpdState(gameState));
        //}

        //private IEnumerator UpdState(GameState gameState)
        //{
        //    yield return new WaitForEndOfFrame();
        //    m_CurrentState = gameState;
        //    if (gameState == GameState.MainMenu)
        //        SceneManager.LoadScene(1);
        //    else if (gameState == GameState.NewGame)
        //        SceneManager.LoadScene(2);
        //    else if (gameState == GameState.EndGame || gameState == GameState.LevelClear)
        //        SceneManager.LoadScene(3);
        //    else
        //    {
        //        MyEventManager.Instance.OnGameStateChanged.Dispatch();
        //    }
        //}

        //public GameState CurrentState
        //{
        //    get
        //    {
        //        return m_CurrentState;
        //    }
        //}
    }
}