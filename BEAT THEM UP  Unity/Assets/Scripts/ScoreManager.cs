using System;
using System.Collections;
using System.Collections.Generic;
using MyPlatformer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : Singleton<ScoreManager>
{
    public event System.Action<int> onScoreUpdated = delegate {  };
    
    private int _score;
    

    public void AddScore(int increment)
    {
        _score += increment;
       onScoreUpdated?.Invoke( _score );
       
       if (_score >= 60)
       {
           SceneManager.LoadScene("Congrat");
       }
    }
    
}