///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: OptionsScript.cs    							       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 29 / 05 / 2018						   /\\\
///\     	    Last entry  - 29 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the options menu.               /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour
{
	void Update ()
    {
        // Workaround for a very annoying bug that was sending dropdown 
        // menus to the back layer when options menu was enabled.
        // This way it's always enabled but with a scale of 0 it can't
        // be interacted with.
        if (MenuScript.isOptionsMenuOpen)
        {
            GetComponent<RectTransform>().localScale = new Vector3(3, 1.5f, 1);
        }
        else
        {
            GetComponent<RectTransform>().localScale = new Vector3(0, 1.5f, 1);
        }
    }
}
