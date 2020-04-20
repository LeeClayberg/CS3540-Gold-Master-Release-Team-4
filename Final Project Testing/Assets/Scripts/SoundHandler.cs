using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    private AudioSource background;
    private AudioSource regularMovement;
    private AudioSource abilities;
    private AudioSource collect;
    public AudioClip running;
    public AudioClip jump;
    public AudioClip grow;
    public AudioClip shrink;
    public AudioClip gravitySwitch;
    public AudioClip collectable;
    public AudioClip shield;
    public AudioClip backgroundMusic;

    public Toggle pauseMenuBackgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        var sources = GetComponents<AudioSource>();
        background = sources[0];
        background.volume = 0.4f;
        regularMovement = sources[1];
        abilities = sources[2];
        abilities.volume = 1f;
        collect = sources[3];
        collect.volume = 0.75f;

        pauseMenuBackgroundMusic.isOn = GameController.backgroundMusicOn;
    }

    public void PlayRunningSound()
    {
        regularMovement.volume = 1f;
        if (regularMovement.clip == jump)
        {
            regularMovement.Stop();
        }
        if (!regularMovement.isPlaying)
        {
            regularMovement.clip = running;
            regularMovement.Play();
        }
    }

    public void PlayJumpSound()
    {
        regularMovement.volume = 3;
        regularMovement.Stop();
        regularMovement.clip = jump;
        regularMovement.Play();
    }

    public void StopSound()
    {
        regularMovement.Stop();
    }

    public void PlayGrowSound()
    {
        abilities.Stop();
        abilities.clip = grow;
        abilities.Play();
    }

    public void PlayShrinkSound()
    {
        abilities.Stop();
        abilities.clip = shrink;
        abilities.Play();
    }

    public void PlayGravitySwitchSound()
    {
        abilities.Stop();
        abilities.clip = gravitySwitch;
        abilities.timeSamples = 0;
        abilities.pitch = 1;  
        abilities.Play();
    }

    public void PlayShieldSound()
    {

        abilities.Stop();
        abilities.clip = shield;
        abilities.Play();
    }

    public void PlayCollectableSound()
    {
        collect.Stop();
        collect.clip = collectable;
        collect.Play();
    }

    public void PlayBackground()
    {
        if (GameController.backgroundMusicOn && !background.isPlaying)
        {
            background.clip = backgroundMusic;
            background.Play();
        }
        else if(!GameController.backgroundMusicOn && background.isPlaying)
        {
            background.Stop();
        }
    }

    public void PauseAllSounds()
    {
        Debug.Log("Paused Sound");
        background.Pause();
        regularMovement.Pause();
        abilities.Pause();
        collect.Pause();
    }

    public void UnPauseAllSounds()
    {
        background.UnPause();
        regularMovement.UnPause();
        abilities.UnPause();
        collect.UnPause();
    }
}
