using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;
using System;

/// <summary>
/// This class is used for Pause the Game with a specified input
/// activating or deactivating the pause UI gameobject
/// and enable or disable the fullscreen
/// </summary>
public class UIMenu : MonoBehaviour
{
    [Tooltip("Input KeyCode")] public KeyCode pause = InputManager.PAUSE;

    [SerializeField] private Vector2Int _windowed = Extension.Constants.ScreenResolution.WINDOWED;

    private Resolution _fullResolution;
    private GameObject _pauseUI;

    bool isPaused = false; //Check variable

    private void Awake()
    {
        _fullResolution = Screen.currentResolution;

        _fullResolution = Screen.currentResolution;

        Screen.SetResolution(_windowed.x, _windowed.y, true);
    }

    private void Start()
    {
        try {
            _pauseUI = transform.Find(Extension.Constants.GameObjectNames.PAUSE).gameObject;
            _pauseUI.SetActive(false); // Disable the GameObject to insure is enabled only when the game is paused
        }
        catch (NullReferenceException)
        {
            Debug.LogError("NullReferenceException: be sure to have a child 'Pause' in " + transform.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pause)) // Input
        {
            isPaused = Pause(isPaused);
            _pauseUI.SetActive(isPaused);
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

