using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    private bool isSettingsOpen = false;

    public TMP_Dropdown resolutionDropDown;

    public FadeSequence fs;

    Resolution[] resolutions;
    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropDown.AddOptions(options);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isSettingsOpen = !isSettingsOpen;
            OpenSettings();
        }
    }

    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void OpenSettings()
    {
        if (isSettingsOpen == true)
        {
            settingsMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        } else if (isSettingsOpen == false)
        {
            settingsMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CloseSettings() 
    {
        isSettingsOpen = false;
        if(isSettingsOpen == false) 
        {
            settingsMenu.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex) 
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ReturnToMenu()
    {
        fs.message = " ";
        SceneManager.LoadScene("StartScreen");
    }
}
