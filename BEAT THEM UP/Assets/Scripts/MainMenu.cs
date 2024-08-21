using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
       SceneManager.LoadScene("HowToPlay"); // Load HowToPlay scene
    }
    public void QuitGame()
    { 
        Application.Quit(); // Quit Game
    }
    public void Credits()
    { 
        SceneManager.LoadScene("CreditScene"); // Load Credit scene
    }
    public void Next()
    {
        SceneManager.LoadScene("GamePlay"); // Load GamePlay scene
    }

    public void Back()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
}
