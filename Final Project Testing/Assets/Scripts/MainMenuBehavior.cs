using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject directionsMenu;
    public Slider sensitivitySlider;
    public Toggle backgroundMusic;

    public void PlayGame()
    {
        GameController.isFlyThrough = false;
        SceneManager.LoadScene(1);
    }

    public void PlayFlyThrough()
    {
        GameController.isFlyThrough = true;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SettingsUp()
    {
        sensitivitySlider.value = GameController.mouseSensitivity;
        backgroundMusic.isOn = GameController.backgroundMusicOn;
        settingsMenu.SetActive(true);
    }

    public void SettingsClose()
    {
        GameController.mouseSensitivity = sensitivitySlider.value;
        GameController.backgroundMusicOn = backgroundMusic.isOn;
        settingsMenu.SetActive(false);
    }

    public void DirectionsUp()
    {
        directionsMenu.SetActive(true);
    }

    public void DirectionsClose()
    {
        directionsMenu.SetActive(false);
    }
}
