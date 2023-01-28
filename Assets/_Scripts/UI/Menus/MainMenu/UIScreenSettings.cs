using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenSettings : MonoBehaviour
{
    public Dropdown dropResolution, dropFPS;
    public Toggle toggleFullscreen;

    public bool Fullscreen { get; set; }

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;


        Resolution[] resolutions = Screen.resolutions;
        System.Array.Reverse(resolutions);

        foreach (Resolution resolution in resolutions)
        {
            dropResolution.options.Add(new Dropdown.OptionData(resolution.width + "x" + resolution.height));
        }

        if (LoadResolution(out int w, out int h, out int sel))
        {
            dropResolution.value = sel;
            toggleFullscreen.isOn = LoadFullscreen();
            Screen.SetResolution(w, h, toggleFullscreen.isOn);
        }
        else
        {
            Screen.SetResolution(Constants.ScreenResolution.WINDOWED.x, Constants.ScreenResolution.WINDOWED.y, false);
        }

        if(LoadRefreshRate(out int hz, out int selectedHz))
        {
            Application.targetFrameRate = hz;
            dropFPS.value = selectedHz;
        }
    }

    #region Setter

    public void SetResolution()
    {
        string[] res = dropResolution.options[dropResolution.value].text.Split('x');

        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);

        SaveResolution(width + ":" + height + ":" + dropResolution.value);
        
        Screen.SetResolution(width, height, Fullscreen);
    }

    public void SetRefreshRate()
    {
        string hz = dropFPS.options[dropFPS.value].text;
        hz = System.Text.RegularExpressions.Regex.Replace(hz, "[^0-9]", ""); // Regular expression

        int fps = int.Parse(hz);

        SaveRefreshRate(fps, dropFPS.value);
        Application.targetFrameRate = fps;
    }

    public void SetFullScreen()
    {
        Fullscreen = !Fullscreen;

        SaveFullscreen(Fullscreen);

        Screen.fullScreen = Fullscreen;
    }

    #endregion

    #region Save

    private void SaveFullscreen(bool fullscreen)
    {
        PlayerPrefs.SetInt(Constants.ScreenResolution.FULLSCREEN_PREF, fullscreen ? 1 : 0);
    }

    private void SaveResolution(string resolution)
    {
        PlayerPrefs.SetString(Constants.ScreenResolution.RESOLUTION_PREF, resolution);
    }

    private void SaveRefreshRate(int hz, int selectedHz)
    {
        PlayerPrefs.SetString(Constants.ScreenResolution.MAXFPS_PREF, hz.ToString() + ":" + selectedHz.ToString());
    }

    #endregion

    #region Load

    private bool LoadFullscreen()
    {
        return PlayerPrefs.GetInt(Constants.ScreenResolution.FULLSCREEN_PREF) == 1 ? true : false;
    }
    private bool LoadResolution(out int width, out int height, out int selectedValue)
    {
        string resolutionString = PlayerPrefs.GetString(Constants.ScreenResolution.RESOLUTION_PREF);

        if (resolutionString != "")
        {
            string[] res = resolutionString.Split(':');
            
            width = int.Parse(res[0]);
            height = int.Parse(res[1]);
            selectedValue = int.Parse(res[2]);

            return true;
        }


        width = 0;
        height = 0;
        selectedValue = 0;
        return false;
    }
    private bool LoadRefreshRate(out int hz, out int selectedHz)
    {
        
        string s = PlayerPrefs.GetString(Constants.ScreenResolution.MAXFPS_PREF);

        if(s != "")
        {
            string[] stringHz = s.Split(':');

            hz = int.Parse(stringHz[0]);
            selectedHz = int.Parse(stringHz[1]);

            return true;
        }

        hz = 0;
        selectedHz = 0;
        return false;
    }

    #endregion
}
