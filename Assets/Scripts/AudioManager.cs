using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> soundEffects;
    [SerializeField] private List<Sound> musicTracks;

    private float musicVolume = 1f;
    private float soundEffectVolume = 1f;
    private List<Sound> pausedSoundEffects;
    public float MusicVolume { get { return musicVolume; } }
    public float SoundEffectVolume { get { return soundEffectVolume; } }

    public static AudioManager instance;

    // Use this for initialization
    private void Awake()
    {
        // delete other instances of the audio manager
        ManageSingleton();

        // create an audio source for each music track
        InitializeAudioSources(musicTracks);

        // create an audio source for each sound effect 
        InitializeAudioSources(soundEffects);

        pausedSoundEffects = new List<Sound>();
    }

    private void Start()
    {
        // update start volume values to 50% to start
        float startVolume = 0.5f;
        instance?.UpdateMusicVolume(startVolume);
        instance?.UpdateSoundEffectVolume(startVolume, playSoundEffect: false);

        // by default, play the main theme if nothing is playing on start
        Sound musicTrack = musicTracks.FirstOrDefault();
        if (musicTrack != null && !musicTrack.source.isPlaying)
        {
            PlayMusicTrack(EnumSoundName.MainTheme);
        }
    }

    #region public functions
    /// <summary>
    /// Used to play a sound effect from the list of sound effects in the audio manager.
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            Debug.Log("Playing: " + soundName.ToString());
            soundEffect.source.Play();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to play a music track from the list of tracks in the audio manager.
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayMusicTrack(EnumSoundName soundName)
    {
        // find any tracks currently playing
        Sound playingMusicTrack = musicTracks.FirstOrDefault(s => s.source.isPlaying == true);

        // find the new track and play it
        Sound musicTrack = musicTracks.FirstOrDefault(s => s.soundName == soundName);
        if (musicTrack != null && musicTrack?.name != playingMusicTrack?.name)
        {
            // if no track is currently playing, simply start the new track
            if (playingMusicTrack == null)
            {
                musicTrack.source.Play();
                Debug.Log("Playing sound track (where no previous sounds exist");
            }
            // otherwise, track currently is playing. Fade out of the first track
            else
            {
                StartCoroutine(FadeOut(playingMusicTrack.source, musicTrack.source));
            }
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to update the volume for all of the music tracks.
    /// </summary>
    /// <param name="volume">A value between 0..1</param>
    public void UpdateMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolume(musicVolume, musicTracks);
    }

    /// <summary>
    /// Used to update the volume for all of the sound effects.
    /// </summary>
    /// <param name="volume">A value between 0..1</param>
    public void UpdateSoundEffectVolume(float volume, bool playSoundEffect)
    {
        soundEffectVolume = volume;
        UpdateVolume(soundEffectVolume, soundEffects);

        // play sound effect
        if (playSoundEffect)
            PlaySoundEffect(EnumSoundName.SoundVolumeChange);
    }

    /// <summary>
    /// Used to stop a sound effect currently playing.
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            soundEffect.source.Stop();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to pause a sound effect currently playing.
    /// </summary>
    /// <param name="soundName"></param>
    public void PauseSoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            soundEffect.source.Pause();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to unpause a sound effect currently playing.
    /// </summary>
    /// <param name="soundName"></param>
    public void UnPauseSoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            soundEffect.source.UnPause();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to pause all sound effects currently playing
    /// </summary>
    /// <param name="soundName"></param>
    public void PauseAllSoundEffects()
    {
        foreach (Sound soundEffect in soundEffects)
        {
            if (soundEffect.source.isPlaying == true)
            {
                soundEffect.source.Pause();
                pausedSoundEffects.Add(soundEffect);
            }
        }
    }

    /// <summary>
    /// Used to unpause all sound effects currently pause
    /// </summary>
    /// <param name="soundName"></param>
    public void UnPauseAllSoundEffects()
    {
        foreach (Sound soundEffect in pausedSoundEffects)
        {
            soundEffect.source.UnPause();
        }

        pausedSoundEffects.Clear();
    }

    public void StopPlayingAllSoundEffects()
    {
        foreach (Sound sound in soundEffects)
        {
            StopSoundEffect(sound.soundName);
        }
    }
    #endregion

    #region private functions 
    /// <summary>
    /// Used to update the volume of the audio source of a list of sounds 
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="sounds"></param>
    void UpdateVolume(float volume, List<Sound> sounds)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume * volume;
        }
    }

    /// <summary>
    /// Used to add an audio source component for each sound in the list of sounds
    /// </summary>
    /// <param name="sounds"></param>
    void InitializeAudioSources(List<Sound> sounds)
    {
        // create an audio source for each sound effect in the list of sounds above
        foreach (Sound sound in sounds)
        {
            if (sound.source == null)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;

                sound.source.loop = sound.loop;
            }
        }
    }

    /// <summary>
    /// Used to manage the singleton. Destroys other instances of audio manager in the scene
    /// to ensure only one audio manager exists.
    /// </summary>
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator FadeOut(AudioSource audioSrc, AudioSource newSound)
    {
        float speed = 0.01f;
        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= speed;
            yield return new WaitForSeconds(0.05f);
        }

        audioSrc.Stop();

        // reset the music volumes
        UpdateVolume(musicVolume, musicTracks);

        newSound.Play();
    }
    #endregion
}
