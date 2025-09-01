using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Score : MonoBehaviour
{
    public int score = 0;

    
    public event Action<int> OnScoreUpdate;

    public void addScore()
    {
        score++;
        OnScoreUpdate?.Invoke(score);
    }

    public int getScore() {  return score; }

    
    
}
