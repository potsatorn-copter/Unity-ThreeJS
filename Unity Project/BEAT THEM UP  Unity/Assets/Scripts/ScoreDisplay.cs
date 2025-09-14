using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Start()
    {
        ScoreManager.instance.onScoreUpdated += UpdateScore;
    }

    public void UpdateScore(int currentScore)
    {
        _scoreText.text = $"{currentScore}";
    }

    private void OnDestroy()
    {
        ScoreManager.instance.onScoreUpdated -= UpdateScore;
    }
    
}