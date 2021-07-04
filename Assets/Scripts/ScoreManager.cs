using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI endGameScoreText;


    public static int Score = 0;
    public static ScoreManager I => _i;
    private static ScoreManager _i;


    private void Awake()
    {
        if (_i != null && _i != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _i = this;
        }
        text = GetComponent<TextMeshProUGUI>();
        HexDestroyer.OnScoreChanged += IncreaseScore;
        endGameScoreText.SetText($"Score : {Score}");
    }

    private void IncreaseScore(int amount)
    {
        SetScore(Score + amount);
    }

    public void SetScore(int score)
    {
        Score = score;
        text.SetText($"Score : {Score}");
        endGameScoreText.SetText($"Score : {Score}");
    }
}
