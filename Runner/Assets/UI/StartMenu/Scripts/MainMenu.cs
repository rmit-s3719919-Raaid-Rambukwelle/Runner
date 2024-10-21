using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.LoadScene("Main", "CrossFade");
    }

    public void Settings() 
    {
        
    }

    public void Inventory() 
    {
    
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
