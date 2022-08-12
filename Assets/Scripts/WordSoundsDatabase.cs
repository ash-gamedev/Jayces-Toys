using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WordSoundsDatabase 
{

    public static AudioClip[] Sounds { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void _Initialize() => Sounds = Resources.LoadAll<AudioClip>("Sounds/");
}