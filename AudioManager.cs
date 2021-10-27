using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

/* AudioManager.cs
 * Max Mitchell
 * Last Updated: Oct. 2021
 * 
 * Purpose: implement AudioManager class to connect sound files from the inspector
 *          to scripting, and allow for other scripts to control pausing, playing,
 *          and stopping these sounds.
 */

public class AudioManager : MonoBehaviour
{
    /* PUBLIC VARIABLES */
    public Sound[] sounds; // fill this in the inspector with all your audio clips!

    // feel free to add more mixer groups if your game requires more channels!
    public AudioMixerGroup mixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup voicesMixer;

    public static AudioManager instance; // class variable; only one AudioManager instance in any scene

    public string musicPlaying = "NotMusic";
    public bool musicIsPaused;
    public Dictionary<string, string> stageMusic = new Dictionary<string, string>();
    public bool sceneLoadOverride = false;

    /* PRIVATE VARIABLES */
    SetVolume setter;

    void Awake()
    {
        setter = FindObjectOfType<SetVolume>();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        musicPlaying = "NotYet";

        DontDestroyOnLoad(gameObject);
        Setup();
    }

    /* Setup()
     * <manages the setup of the mixers, and all audio files into the
     * AudioManager interface>
     */
    private void Setup()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = mixer;

            // NAMING CONVENTION:
            // beginning of each clips name indicates the type of audio it is
            // and which mixer group it will be routed to
            if (s.name.Length > 5 && s.name.Substring(0, 5) == "Music")
            {
                s.source.outputAudioMixerGroup = musicMixer;
            }
            if (s.name.Length > 3 && s.name.Substring(0, 3) == "SFX")
            {
                s.source.outputAudioMixerGroup = sfxMixer;
            }
            if (s.name.Length > 5 && s.name.Substring(0, 5) == "Voice")
            {
                s.source.outputAudioMixerGroup = voicesMixer;
            }

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop ;
        }
    }

    private void Start()
    {
        // for each scene, must add it's appropriate music to the stageMusic
        // TODO: Change this to have the names of your songs and your scenes!
        stageMusic.Add("Level", "MusicTitle");

        SceneManager.sceneLoaded += SceneLoaded;
        SceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    /* SceneLoaded(Scene s, LoadSceneMode m)
     * <called automatically by Max's script SceneLoader.cs,a and manages the 
     * changing of scenes while keeping consistent music>
     */
    private void SceneLoaded(Scene s, LoadSceneMode m)
    {
        if (!sceneLoadOverride)
        {
            string music;
            // Find appropriate music for scene
            stageMusic.TryGetValue(s.name, out music);

            // stop whatever is currently playing unless they are the same song
            if (musicPlaying.Substring(0, 5) == "Music" && music != musicPlaying)
            {
                Stop(musicPlaying);
            }

            // Now, play music for the stage you have woken up in
            if (music != musicPlaying)
            {
                Play(music, 1);
            }
        }
        sceneLoadOverride = false;
    }

    /* Play(string name)
     * <override for Play that allows playing audio at the default pitch>
     */
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        if (name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    /* Play(string name, float pitch)
     * <plays any sound you have put into the AudioManager. selects sound based
     * on string name, and can play at varying pitches given in the float pitch>
     */
    public void Play(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        s.source.pitch = pitch;

        if(name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }

        s.source.Play();
    }

    /* PlayFromSource(string name, float pitch, float volume, AudioSource audiosource)
     * <functions the same as Play, but allows playing from a localized AudioSource instead of the
     * general MainCamera's AudioSource. also takes a float for variable volume>
     */
    public void PlayFromSource(string name, float pitch, float volume, AudioSource audiosource)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        if (name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }


        if (!s.source.isPlaying)
        {
            audiosource.clip = s.source.clip;
            audiosource.outputAudioMixerGroup = s.source.outputAudioMixerGroup;
            audiosource.volume = s.source.volume;
            audiosource.pitch = pitch;
            audiosource.loop = s.source.loop;
            audiosource.spatialBlend = 1f;
            audiosource.Play();
        }
    }

    /* Stop(string name)
     * <stops the sound given by the string name>
     */
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    /* Pause(string name)
     * <pauses the sound given by the string name. this is different from
     * stopping, since pause allows you to resume the sound clip where
     * you paused it from>
     */
    public void Pause(string name)
    {
        musicIsPaused = true;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    /* PauseAll()
     * <pauses all sounds in the scene>
     */
    public void PauseAll()
    {
        musicIsPaused = true;
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }

        // also pause from every audiosource in game
        foreach (AudioSource AS in FindObjectsOfType<AudioSource>())
        {
            AS.Pause();
        }
    }

    /* Unpause(string name)
     * <unpauses the sound given by string name>
     */
    public void UnPause(string name)
    {
        musicIsPaused = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.UnPause();
    }

    /* UnPauseAll()
     * <unpauses all sounds in the scene>
     */
    public void UnPauseAll()
    {
        musicIsPaused = false;
        foreach (Sound s in sounds)
        {
            s.source.Pause();
            s.source.UnPause();
        }

        // also unpause from every audiosource in game
        foreach (AudioSource AS in FindObjectsOfType<AudioSource>())
        {
            AS.Pause();
            AS.UnPause();
        }
    }

    /* UnPauseMusic()
     * <unpauses just the music that is currently playing>
     */
    public void UnPauseMusic()
    {
        UnPause(musicPlaying);
    }

    
}
