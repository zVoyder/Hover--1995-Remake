using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource masterTry, musicTry, effectTry;
    public Slider masterSlider, musicSlider, effectsSlider;


    private void Awake()
    {

        if (LoadVolume(out float master, out float music, out float sfx))
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
            mixer.SetFloat("Master", 0);
            mixer.SetFloat("Music", 0);
            mixer.SetFloat("Effects", 0);
        }

        masterTry.Stop();
        musicTry.Stop();
        effectTry.Stop();
    }


    public void SetMaster()
    {
        mixer.SetFloat("Master", masterSlider.value);

        mixer.GetFloat("Master", out float v);

        masterTry.Play();
        SaveVolume();
    }

    public void SetMusic()
    {
        mixer.SetFloat("Music", musicSlider.value);
        musicTry.Play();
        SaveVolume();
    }

    public void SetEffects()
    {
        mixer.SetFloat("Effects", effectsSlider.value);
        effectTry.Play();
        SaveVolume();
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetString(Constants.Audio.VOLUME_PREF, (int)masterSlider.value + ":" + (int)musicSlider.value + ":" + (int)effectsSlider.value);
    }

    private bool LoadVolume(out float master, out float music, out float effects)
    {
        string volString = PlayerPrefs.GetString(Constants.Audio.VOLUME_PREF);

        if(volString != "")
        {
            string[] v = volString.Split(':');

            master = (float)int.Parse(v[0]);
            music = (float)int.Parse(v[1]);
            effects = (float)int.Parse(v[2]);

            return true;
        }

        master = 0f;
        music = 0f;
        effects = 0f;
        return false;
    }
}
