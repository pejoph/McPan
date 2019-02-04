///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: SettingsScript.cs        							   /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 29 / 05 / 2018						   /\\\
///\     	    Last entry  - 29 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the options menu with controls  /\\\
///\            for resolution, fullscreen and video quality.          /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // Sets our resolutions array to the resolutions of the screen in use.
        resolutions = Screen.resolutions;

        // Clears my test values.
        resolutionDropdown.ClearOptions();

        // Creates list of strings to store resolution names.
        List<string> options = new List<string>();

        // Create index used to show the current screen resolution when launching the game.
        int currentResolutionIndex = 0;
        // Loop through the resolutions and update our string list. 
        for (int i = 0; i < resolutions.Length; i++)
        {
            string sOption = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(sOption);

            // When the current resolution is found, store that index value.
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add all the elements of our list to the dropdown in the menu.
        resolutionDropdown.AddOptions(options);
        // Set the currently displayed value to the current screen resolution.
        resolutionDropdown.value = currentResolutionIndex;
        // Refresh the value so it displays correctly.
        resolutionDropdown.RefreshShownValue();
    }

    // Called when the quality dropdown is adjusted. Changes the video quality.
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Called when the fullscreen toggle is pressed. Maximises/middlemises (yes, that's now a word) the game window.
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Called when the resolution dropdown is adjusted. Changes the resolution.
    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }
}
