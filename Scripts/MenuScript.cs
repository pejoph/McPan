///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: MenuScript.cs    								       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 28 / 05 / 2018						   /\\\
///\     	    Last entry  - 29 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the menu screen.                /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;
    public Canvas startMenu;
    public Canvas optionsMenu;
    public Canvas exitMenu;
    public Text mcPanTitle;
    public Slider loadingBar;
    public Text loadingPercentage;

    public static bool isOptionsMenuOpen;

    void Start()
    {
        // Allow use of the on-screen buttons
        playButton.enabled = true;
        optionsButton.enabled = true;
        exitButton.enabled = true;
        // Don't display the options or exit menus.
        startMenu.enabled = true;
        isOptionsMenuOpen = false;
        //optionsMenu.GetComponent<RectTransform>().localScale = new Vector3(3, 1.5f, 1);
        exitMenu.enabled = false;
        // Set the standard timescale (without this nothing is animated in the menu screen if you pause the game and then return to the menu).
        Time.timeScale = 1.0f;
        // Position the loading bar off-screen.
        loadingBar.transform.position = new Vector3(loadingBar.transform.position.x, -400.0f, loadingBar.transform.position.z);
    }

    void Update()
    {
        // When escape is pressed, bring up the exit menu - unless a menu is already displaying over the start menu. In which case, close that menu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exitMenu.enabled && !isOptionsMenuOpen)
            {
                Exit();
            }
            else
            {
                Start();
            }
        }

        // Set the title shadows to follow the cursor.
        mcPanTitle.GetComponents<Shadow>()[1].effectDistance = new Vector2(-2 + (Input.mousePosition.x - Screen.width / 2) / 100,  1 + (Input.mousePosition.y - Screen.height * 345 / 450) / 100);
        mcPanTitle.GetComponents<Shadow>()[2].effectDistance = new Vector2( 1 + (Input.mousePosition.x - Screen.width / 2) / 100,  2 + (Input.mousePosition.y - Screen.height * 345 / 450) / 100);
        mcPanTitle.GetComponents<Shadow>()[3].effectDistance = new Vector2( 2 + (Input.mousePosition.x - Screen.width / 2) / 100, -1 + (Input.mousePosition.y - Screen.height * 345 / 450) / 100);
        mcPanTitle.GetComponents<Shadow>()[4].effectDistance = new Vector2(-1 + (Input.mousePosition.x - Screen.width / 2) / 100, -2 + (Input.mousePosition.y - Screen.height * 345 / 450) / 100);

    }

    // Called when the options button is pressed. Disables start menu and brings up the option menu.
    public void Options()
    {
        startMenu.enabled = false;
        isOptionsMenuOpen = true;
        //optionsMenu.GetComponent<RectTransform>().localScale = new Vector3(3, 1.5f, 1);
    }

    // Called when the options menu is closed. Enables start menu and removes the option menu.
    public void OptionsClose()
    {
        startMenu.enabled = true;
        isOptionsMenuOpen = false;
        //optionsMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 1.5f, 1);
    }
    
    // Called when the exit button is pressed. Disables start menu buttons and brings up the exit menu.
    public void Exit()
    {
        playButton.enabled = false;
        optionsButton.enabled = false;
        exitButton.enabled = false;
    
        exitMenu.enabled = true;
    }
    
    // Called when the yes button is pressed on the exit menu. Quits application.
    public void ExitYes()
    {
        Application.Quit();
    }
    
    // Called when the no button is pressed on the exit menu. Enables start menu buttons and removes the exit menu.
    public void ExitNo()
    {
        playButton.enabled = true;
        optionsButton.enabled = true;
        exitButton.enabled = true;
    
        exitMenu.enabled = false;
    }
    
    // Called when the play button is pressed. Disables start menu buttons and moves the loading bar into frame. Also starts a coroutine. 
    public void LoadLevel(int sceneIndex)
    {
        playButton.enabled = false;
        optionsButton.enabled = false;
        exitButton.enabled = false;
    
        loadingBar.transform.position = new Vector3(loadingBar.transform.position.x, 40.0f, loadingBar.transform.position.z);
    
        StartCoroutine(LoadAsync(sceneIndex));
    }
    
    // Called alongside LoadLevel(). Starts loading the game level asynchronously and increments the loading bar and percentage.
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    
        while (!operation.isDone)
        {
            // In my experience, the level loads instantly (which is 90% of the progress) and then takes about 15 seconds to activate all the necessary files (the other 10%) so here I increase the loading bar percentage by 1% every 1.5 seconds so it doesn't look like the game has crashed.
            loadingBar.value = operation.progress / .9f;
            loadingPercentage.text = Mathf.Floor(100.0f * loadingBar.value) + "%";
    
            yield return null;
        }
    }
}
