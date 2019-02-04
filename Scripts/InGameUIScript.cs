///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: InGameUIScript.cs  								       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 28 / 05 / 2018						   /\\\
///\     	    Last entry  - 29 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the in-game menus.              /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIScript : MonoBehaviour
{
    [SerializeField] private Text youWin;
    [SerializeField] private Text youLose;
    [SerializeField] private Text paused;
    [SerializeField] private Canvas winScreen;
    [SerializeField] private Canvas loseScreen;
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private Slider batterySlider;
    
    public Text battery;
    public Text speed;
    public Text torchAngle;
    public Text spyMode;
    public Text range;

    public static float batteryAlpha = 0;
    public static float speedAlpha = 0;
    public static float torchAngleAlpha = 0;
    public static float spyModeAlpha = 0;
    public static float rangeAlpha = 0;


    private void Start()
    {
        winScreen.enabled = false;
        loseScreen.enabled = false;
        pauseMenu.enabled = false;
    }
    private void Update()
    {
        // Animate UI titles.
        // Win screen.
        youWin.GetComponents<Shadow>()[0].effectDistance = new Vector2(-2 - Mathf.Cos(Time.time * 2 * Mathf.PI), 1 + Mathf.Sin(Time.time * 2 * Mathf.PI));
        youWin.GetComponents<Shadow>()[1].effectDistance = new Vector2(1 - Mathf.Cos(Time.time * 2 * Mathf.PI), 2 + Mathf.Sin(Time.time * 2 * Mathf.PI));
        youWin.GetComponents<Shadow>()[2].effectDistance = new Vector2(2 - Mathf.Cos(Time.time * 2 * Mathf.PI), -1 + Mathf.Sin(Time.time * 2 * Mathf.PI));
        youWin.GetComponents<Shadow>()[3].effectDistance = new Vector2(-1 - Mathf.Cos(Time.time * 2 * Mathf.PI), -2 + Mathf.Sin(Time.time * 2 * Mathf.PI));
        // Lose screen.
        youLose.GetComponents<Shadow>()[0].effectDistance = new Vector2(-2 - Mathf.Cos(Time.time * 1 * Mathf.PI), 1 + Mathf.Sin(Time.time * 1 * Mathf.PI));
        youLose.GetComponents<Shadow>()[1].effectDistance = new Vector2(1 - Mathf.Cos(Time.time * 1 * Mathf.PI), 2 + Mathf.Sin(Time.time * 1 * Mathf.PI));
        youLose.GetComponents<Shadow>()[2].effectDistance = new Vector2(2 - Mathf.Cos(Time.time * 1 * Mathf.PI), -1 + Mathf.Sin(Time.time * 1 * Mathf.PI));
        youLose.GetComponents<Shadow>()[3].effectDistance = new Vector2(-1 - Mathf.Cos(Time.time * 1 * Mathf.PI), -2 + Mathf.Sin(Time.time * 1 * Mathf.PI));
        // Pause menu.
        paused.GetComponents<Shadow>()[0].effectDistance = new Vector2(-2 - Mathf.Cos(Time.time * 1.5f * Mathf.PI), 1 + Mathf.Sin(Time.time * 1.5f * Mathf.PI));
        paused.GetComponents<Shadow>()[1].effectDistance = new Vector2(1 - Mathf.Cos(Time.time * 1.5f * Mathf.PI), 2 + Mathf.Sin(Time.time * 1.5f * Mathf.PI));
        paused.GetComponents<Shadow>()[2].effectDistance = new Vector2(2 - Mathf.Cos(Time.time * 1.5f * Mathf.PI), -1 + Mathf.Sin(Time.time * 1.5f * Mathf.PI));
        paused.GetComponents<Shadow>()[3].effectDistance = new Vector2(-1 - Mathf.Cos(Time.time * 1.5f * Mathf.PI), -2 + Mathf.Sin(Time.time * 1.5f * Mathf.PI));

        // Enable/disable pause menu when pressing 'P' or 'ESC'.
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.enabled = !pauseMenu.enabled;
        }

        // Pause game when pause menu is open.
        if (pauseMenu.enabled)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Open win screen upon game completion.
        if (MapGenerationController.hasWonGame)
        {
            winScreen.enabled = true;
        }
        else
        {
            winScreen.enabled = false;
        }

        // Open lose screen upon game loss.
        if (MapGenerationController.hasLostGame)
        {
            loseScreen.enabled = true;
        }
        else
        {
            loseScreen.enabled = false;
        }

        // Set the slider to the battery value.
        batterySlider.GetComponent<Slider>().value = MapGenerationController.batteryLife;

        // Update the text alpha values.
        battery.color = new Color(battery.color.r, battery.color.g, battery.color.b, batteryAlpha);
        speed.color = new Color(speed.color.r, speed.color.g, speed.color.b, speedAlpha);
        torchAngle.color = new Color(torchAngle.color.r, torchAngle.color.g, torchAngle.color.b, torchAngleAlpha);
        spyMode.color = new Color(spyMode.color.r, spyMode.color.g, spyMode.color.b, spyModeAlpha);
        range.color = new Color(range.color.r, range.color.g, range.color.b, rangeAlpha);
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
        MapGenerationController.hasWonGame = false;
        MapGenerationController.hasLostGame = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        pauseMenu.enabled = false;
    }
}
