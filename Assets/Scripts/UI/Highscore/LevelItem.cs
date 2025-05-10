using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BallDrop
{
    public class LevelItem : MonoBehaviour
    {
        public int Level;
        public TextMeshProUGUI LevelText;

        public void SetLevelText()
        {
            LevelText.text = "Level - " + Level;
        }

        public void LoadLevelLeaderboard()
        {
            MyEventManager.LoadLevelLeaderboard.Dispatch(Level);
        }
    }
}