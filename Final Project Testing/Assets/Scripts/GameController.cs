using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public static bool isGameOver = false;

    public Text hint;
    public Text timeText;
    private float time;
    public GameObject backgroundPanel;

    public GameObject shrink;
    public GameObject gravity;
    public GameObject doubleJump;
    public GameObject diamond;

    private bool isShrinkHintShowed = false;
    private bool isGravityHintShowed = false;
    private bool isDoubleJumpHintShowed = false;

    public NewCharacterController cc;

    public GameObject player;
    public GameObject mainUI;
    public GameObject pathAndCamera;

    public static bool isFlyThrough = false;
    public static float mouseSensitivity = 25;
    public static bool backgroundMusicOn = true;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        isGameOver = false;
        StartHint();
        if(isFlyThrough)
        {
            setFlyThroughMode();
        }
        Invoke("DisableText", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                ReloadLevel();
            }
        }
        else
        {
            time += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            timeText.text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        if (!isShrinkHintShowed && shrink == null)
        {
            ShrinkHint();
        }
        if (!isGravityHintShowed && gravity == null)
        {
            GravityHint();
        }
        if (!isDoubleJumpHintShowed && doubleJump == null)
        {
            DoubleJumpHint();
        }

        if (shrink == null && gravity == null && doubleJump == null && diamond == null)
        {
            LevelBeat();
            cc.DontMove();
        }
    }

    void StartHint()
    {
        hint.text = "Collect all coins to find your new abilities";
        hint.gameObject.SetActive(true);
    }

    public void LevelLost()
    {
        isGameOver = true;
        hint.text = "You are dead" + '\n' + "Press G to restart";
        backgroundPanel.SetActive(true);
        hint.gameObject.SetActive(true);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        hint.text = "You Win!" + '\n' + "Press G to restart";
        hint.gameObject.SetActive(true);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }

    void DisableText()
    {
        hint.gameObject.SetActive(false);
    }

    void ShrinkHint()
    {
        hint.text = "You can shrink your size now" + '\n' + "Press R to switch your character" + '\n' + "Press F to use your ability";
        hint.gameObject.SetActive(true);
        Invoke("DisableText", 5f);
        isShrinkHintShowed = true;
    }

    void GravityHint()
    {
        hint.text = "You can change the gravity now" + '\n' + "Press R to switch your character" + '\n' + "Press F to use your ability";
        hint.gameObject.SetActive(true);
        Invoke("DisableText", 5f);
        isGravityHintShowed = true;
    }

    void DoubleJumpHint()
    {
        hint.text = "You can double jump now" + '\n' + "Press R to switch your character" + '\n' + "Press Space to jump a second time in air";
        hint.gameObject.SetActive(true);
        Invoke("DisableText", 5f);
        isDoubleJumpHintShowed = true;
    }

    public void setFlyThroughMode()
    {
        player.SetActive(false);
        mainUI.SetActive(false);
        pathAndCamera.SetActive(true);
    }
}
