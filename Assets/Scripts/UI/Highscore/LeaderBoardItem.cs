using BallDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardItem : MonoBehaviour
{

    public int score;
    public string playerName;
    public TextMeshProUGUI PlayerName, PlayerScore;

    public void SetUI()
    {
        PlayerName.text = playerName;
        PlayerScore.text = score.ToString();
    }


}
