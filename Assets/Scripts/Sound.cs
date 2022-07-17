using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound")]
public class Sound : ScriptableObject
{
    // to identify the sound
    public EnumSoundName soundName;

    // audio clip to play
    public AudioClip clip;

    // default volume
    [Range(0f, 1f)]
    public float volume = 0.65f;

    // default pitch 
    //[Range(.1f, 3f)] (hide for now, likely don't need values beside 1)
    [HideInInspector] public float pitch = 1;

    // should the sound repeat
    public bool loop;

    // the audio source where the sound will play from 
    [HideInInspector]
    public AudioSource source;
}
