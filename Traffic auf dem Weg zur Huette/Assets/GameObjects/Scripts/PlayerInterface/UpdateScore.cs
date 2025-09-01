using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    private Score score;
    private Text text;
    private void Start()
    {
        score = FindObjectOfType<Score>();
        text = GetComponent<Text>();
        score.OnScoreUpdate += HandleScoreUpdate;
    }

    public void HandleScoreUpdate(int score)
    {
        text.text = score.ToString();
    }
}
