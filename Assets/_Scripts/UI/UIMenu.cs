using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneSetting;

/// <summary>
/// This class is used for Pause the Game with a specified input
/// activating or deactivating the pause UI gameobject
/// and enable or disable the fullscreen
/// </summary>
public class UIMenu : MonoBehaviour
{
    [Tooltip("GameObject I want to enable when I pause the game")]public GameObject pauseUI;
    [Tooltip("Input KeyCode")] public KeyCode pause = InputManager.PAUSE; 

    bool isPaused = false; //Check variable
    private Resolution _fullResolution = SceneSetting.ScreenResolution.STARTED;

    private void Awake()
    {
        pauseUI.SetActive(false); // Disable the GameObject to insure is enabled only when the game is paused
        _fullResolution = Screen.currentResolution;
        Screen.SetResolution(800, 600, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(pause)) // Input
        {
            isPaused = Pause(isPaused);
            pauseUI.SetActive(isPaused);
           
        }

        if (Input.GetKeyDown(InputManager.FULLSCREEN))
        {
            Screen.SetResolution(_fullResolution.width, _fullResolution.height, true);
        }
    }

    /// <summary>
    /// Pause the game by setting the time scale
    /// </summary>
    /// <param name="pause">Is the game paused now?</param>
    /// <returns></returns>
    private bool Pause(bool pause)
    {
#if DEBUG
        Debug.Log("PAUSE");
#endif
        Time.timeScale = pause ? 1 : 0; // Scale time based if the game is paused or not
        return !pause;
    }
}

