using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundsDatabase 
{
    public static AudioClip[] WordSounds { get; private set; }
    public static AudioClip[] CongratsSounds { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void _Initialize()
    {
        WordSounds = Resources.LoadAll<AudioClip>("WordSounds/");
        CongratsSounds = Resources.LoadAll<AudioClip>("CongratsSounds/");
    }
}