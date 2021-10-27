
using UnityEngine.Audio;
using UnityEngine;

/* Sound.cs
 * Max Mitchell
 * Last Updated: Oct. 2021
 * 
 * Purpose: simple Sound class holds all requisite components of what we consider
 *          to be a "sound" in the AudioManager class, to represent audio. All
 *          sounds must have a string for a name and an AudioClip for the clip
 *          to be played. Other features are optional.
 */

[System.Serializable]
public class Sound 
{
    public AudioClip clip;

    public string name; // see AudioManager class for naming conventions
    
    [Range(0f, 1f)]
    public float volume; // set default volume

    [Range(.1f, 3f)]
    public float pitch; // set default pitch

    public bool loop; // set clip to loop upon completion

    [HideInInspector]
    public AudioSource source; // set source for clip to play from in scene
}
