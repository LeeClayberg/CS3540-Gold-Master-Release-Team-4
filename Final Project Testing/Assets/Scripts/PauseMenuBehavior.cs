using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;
    public GameObject mainCamera;
    public Text timeText;
    public Text pauseMenuTime;
    public Slider pauseMenuSensitivity;
    public Toggle pauseMenuBackgroundMusic;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (!GameController.isFlyThrough) {
            pauseMenuTime.text = timeText.text;
            GameController.mouseSensitivity = pauseMenuSensitivity.value;
            GameController.backgroundMusicOn = pauseMenuBackgroundMusic.isOn;
        }
    }

    void PauseGame()
    {
        mainCamera.GetComponent<SoundHandler>().PauseAllSounds();

        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        mainCamera.GetComponent<SoundHandler>().UnPauseAllSounds();

        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
