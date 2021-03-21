using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public Text textScore;

    public void UpdateScore(int amount)
    {
        score += amount;
        textScore.text = "Score: "+score.ToString();
    }
}
