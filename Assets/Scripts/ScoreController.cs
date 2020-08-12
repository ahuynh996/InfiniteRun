using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    static int score = 0;

    public Text scoreText;

    void Update()
    {

        scoreText.text = score.ToString();
    }
    public static void IncrementScore()
    {
        score++;
    }
}
