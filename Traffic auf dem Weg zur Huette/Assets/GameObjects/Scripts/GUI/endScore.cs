using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endScore : MonoBehaviour
{
    private Text text;
    private int score;
    private Score sc;

    private void Start()
    {
        text = GetComponent<Text>();
        sc = FindObjectOfType<Score>();
        this.score = sc.getScore();
        text.text = "Score: " + score;
    }




}
