using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/* SetVolume.cs
 * Max Mitchell
 * Last Updated: Oct. 2021
 * 
 * Purpose: SetVolume class acts as an interface for client scripts to modify 
 *          volume of various Sounds in an AudioManager instance. Has 
 *          functionality for adjusting volumes, and a simple fade. Primarily 
 *          designed for use with the Slider class, and adding volume sliders 
 *          to a game's GUI
 */

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    // AudioManager has three channels to start. Feel free
    // to add or remove channels as needed.
    public Slider Master;
    public Slider Music;
    public Slider SFX;
    public Slider Voices;

    private void Start()
    {
        // setup sliders based on current mixer levels
        float temp;

        mixer.GetFloat("MasterVolume", out temp);
        Master.value = MixerToSliderValue(temp);

        mixer.GetFloat("MusicVolume", out temp);
        Music.value = MixerToSliderValue(temp);

        mixer.GetFloat("SFXVolume", out temp);
        SFX.value = MixerToSliderValue(temp);

        mixer.GetFloat("VoicesVolume", out temp);
        Voices.value = MixerToSliderValue(temp);
    }

    float MixerToSliderValue(float mixerValue)
    {
        // reverse process used to turn slider value into mixer value
        float sliderValue = mixerValue / 20;
        sliderValue = Mathf.Pow(10, sliderValue);
        return sliderValue;
    }

    // Call these setter functions from whatever scripting component you want
    // to change the audio levels. Recommended: Slider
    public void SetLevel(float sliderValue) { mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20); }
    public void SetMusicLevel(float sliderValue) { mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); }
    public void SetSFXLevel(float sliderValue) { mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20); }
    public void SetVoicesLevel(float sliderValue) { mixer.SetFloat("VoicesVolume", Mathf.Log10(sliderValue) * 20); }

    // Use to fade music to specific volume
    public IEnumerator FadeMusic(float time, float goal)
    {
        float temp;
        mixer.GetFloat("MusicVolume", out temp);
        temp = MixerToSliderValue(temp);
        float change = temp;

        float velocity = 10.0f;
        float i = time;
        while (!Mathf.Approximately(change, goal))
        {
            change = Mathf.SmoothDamp(temp, goal, ref velocity, time / 10);
            SetMusicLevel(change);
            print(change);

            i -= Time.deltaTime;
            yield return null;
        }
        SetMusicLevel(goal);
    }
}