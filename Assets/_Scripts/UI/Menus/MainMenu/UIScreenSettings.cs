using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenSettings : MonoBehaviour
{
    public Dropdown dropResolution, dropFPS;
    public Toggle toggleFullscreen;

    public bool Fullscreen { get; set; }

    public void Awake()
    {
        QualitySettings.vSyncCount = 0; // Disable V-Sync to allow the FPS Cap
         
        foreach (string resolution in GetCurrentResolutions())
        {
            dropResolution.options.Add(new Dropdown.OptionData(resolution));
        }

        if (SaveManager.Screen.LoadResolution(out int w, out int h, out int sel))
        {
            dropResolution.value = sel;
            toggleFullscreen.isOn = SaveManager.Screen.LoadFullscreen();
            Screen.SetResolution(w, h, toggleFullscreen.isOn);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }

        if(SaveManager.Screen.LoadRefreshRate(out int hz, out int selectedHz))
        {
            Application.targetFrameRate = hz;
            dropFPS.value = selectedHz;
        }
    }

    /// <summary>
    /// Get the current available resolutions
    /// as array of strings
    /// </summary>
    /// <returns>Available resolutions</returns>
    private string[] GetCurrentResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        System.Array.Reverse(resolutions);

        List<string> resList = new List<string>(); // Creating a list of strings

        foreach (Resolution res in resolutions) // In this list of string only add the witdth and the height
        {
            resList.Add(res.width + "x" + res.height);
        }

        return resList.ToArray().Distinct().ToArray(); // Distinct() because there are resolutions' duplicates due the refresh rate
    }

    #region Setter

    /// <summary>
    /// Set the resolution of the screen
    /// </summary>
    public void SetResolution()
    {
        string[] res = dropResolution.options[dropResolution.value].text.Split('x');

        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);

        SaveManager.Screen.SaveResolution(width + ":" + height + ":" + dropResolution.value);
        
        Screen.SetResolution(width, height, Fullscreen);
    }

    /// <summary>
    /// Set the refresh rate of the screen
    /// </summary>
    public void SetRefreshRate()
    {
        string hz = dropFPS.options[dropFPS.value].text;
        hz = System.Text.RegularExpressions.Regex.Replace(hz, "[^0-9]", ""); // Regular expression

        int fps = int.Parse(hz);

        SaveManager.Screen.SaveRefreshRate(fps, dropFPS.value);
        Application.targetFrameRate = fps;
    }


    /// <summary>
    /// Toggle for setting the prefered fullscreen mode
    /// </summary>
    public void SetFullScreen()
    {
        Fullscreen = !Fullscreen;

        SaveManager.Screen.SaveFullscreen(Fullscreen);

        Screen.fullScreen = Fullscreen;
    }

    #endregion
}
