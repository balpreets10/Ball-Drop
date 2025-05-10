using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BallDrop
{
    public class CoinManager : SingletonMonoBehaviour<CoinManager>
    {
        private int m_Coins = 0;
        private int m_PreviousValue = -1;


        private void Start()
        {
            UpdatePreviousValue();
            Debug.unityLogger.Log(GameData.TAG, "coins = " + m_Coins);
        }

        private void OnEnable()
        {
            MyEventManager.Instance.UpdateCoins.AddListener(UpdateCoins);
            MyEventManager.Instance.OnCompletedAwardAd.AddListener(AwardCoins);
        }

        private void OnDisable()
        {
            if (MyEventManager.Instance != null)
            {
                MyEventManager.Instance.UpdateCoins.RemoveListener(UpdateCoins);
                MyEventManager.Instance.OnCompletedAwardAd.RemoveListener(AwardCoins);
            }

        }

        private void UpdatePreviousValue()
        {
            m_PreviousValue = GetCoins() - 1;
        }

        private void UpdateCoins()
        {
            if (m_PreviousValue == m_Coins - 1)
            {
                m_Coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0);
                m_PreviousValue = m_Coins;
                PreferenceManager.Instance.IncrementIntPref(PrefKey.Coins, 0);
                MyEventManager.Instance.OnCoinsUpdated.Dispatch(++m_Coins);
            }
        }

        private void AwardCoins(int amount)
        {
            print("Awarded coins = " + amount);
            m_Coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0);
            PreferenceManager.Instance.UpdateIntPref(PrefKey.Coins, m_Coins + amount);
            MyEventManager.Instance.OnCoinsAwarded.Dispatch(m_Coins + amount);
            UpdatePreviousValue();
            Debug.unityLogger.Log(GameData.TAG, "total coins = " + m_Coins);
        }

        public void RefreshCoins(int value)
        {
            PreferenceManager.Instance.UpdateIntPref(PrefKey.Coins, value);
            UpdatePreviousValue();
        }

        public int GetCoins()
        {
            return m_Coins = PreferenceManager.Instance.GetIntPref(PrefKey.Coins, 0);
        }
    }

}