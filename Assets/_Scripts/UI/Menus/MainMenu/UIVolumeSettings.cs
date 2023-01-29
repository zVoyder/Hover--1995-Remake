using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


/// <summary>
/// This class handles the Volume settings of the game in the MainMenu,
/// It is responsible for managing the volume of Master, Music and Effects
/// </summary>
public class UIVolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource masterTry, musicTry, effectTry;
    public Slider masterSlider, musicSlider, effectsSlider;


    private void Awake()
    {

        if (SaveManager.Audio.LoadVolume(out float master, out float music, out float sfx))
        {
            mixer.SetFloat("Master", master);
            mixer.SetFloat("Music", music);
            mixer.SetFloat("Effects", sfx);

            masterSlider.value = master;
            musicSlider.value = music;
            effectsSlider.value = sfx;
        }
        else
        {
            // If there were no saved values then we set the volume to 0 (that means +0dB)
            mixer.SetFloat("Master", 0);
            mixer.SetFloat("Music", 0);
            mixer.SetFloat("Effects", 0);
        }

        masterTry.Stop(); // This way on application start you cant hear the sound on changing the slider
        musicTry.Stop();
        effectTry.Stop();
    }

    /// <summary>
    /// This method is used to set the master volume
    /// </summary>
    public void SetMaster()
    {
        mixer.SetFloat("Master", masterSlider.value);

        mixer.GetFloat("Master", out float v);

        masterTry.Play();
        SaveManager.Audio.SaveVolume((int)masterSlider.value, (int)musicSlider.value, (int)effectsSlider.value);
    }

    /// <summary>
    /// This method is used to set the music volume
    /// </summary>
    public void SetMusic()
    {
        mixer.SetFloat("Music", musicSlider.value);
        musicTry.Play();
        SaveManager.Audio.SaveVolume((int)masterSlider.value, (int)musicSlider.value, (int)effectsSlider.value);
    }

    /// <summary>
    /// This method is used to set the effects volume
    /// </summary>
    public void SetEffects()
    {
        mixer.SetFloat("Effects", effectsSlider.value);
        effectTry.Play();
        SaveManager.Audio.SaveVolume((int)masterSlider.value, (int)musicSlider.value, (int)effectsSlider.value);
    }

}
