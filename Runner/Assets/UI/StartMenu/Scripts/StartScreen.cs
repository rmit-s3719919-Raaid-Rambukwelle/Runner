using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public MainMenu mainMenu;
    void Update()
    {
        if (Input.anyKeyDown) 
        {
            if (mainMenu != null)
            {
                mainMenu.StartGame();
            } else 
                {
                Debug.LogError("MainMenu reference not set");
                }
        }
    }
}
