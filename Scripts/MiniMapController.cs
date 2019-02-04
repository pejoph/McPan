///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: MiniMapController.cs    							   /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 28 / 05 / 2018						   /\\\
///\     	    Last entry  - 28 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the mini map.                   /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public GameObject player;
    public GameObject blotty;
    public GameObject winky;
    public GameObject magenty;
    public GameObject bonnie;
    public GameObject myLight;
    public GameObject icon;

	void Start ()
    {
        // Set the relevent marker colours.
        if (tag == "Blotty")
        {
            myLight.GetComponent<Light>().color = new Color(0, 1, 1);
        }
        else if (tag == "Winky")
        {
            myLight.GetComponent<Light>().color = new Color(214.0f / 255.0f, 0, 0);
        }
        else if (tag == "Magenty")
        {
            myLight.GetComponent<Light>().color = new Color(1, 79.0f / 255.0f, 215.0f / 255.0f);
        }
        else if (tag == "Bonnie")
        {
            myLight.GetComponent<Light>().color = new Color(1, 111.0f / 255.0f, 0);
        }
    }

    private void LateUpdate()
    {
        // Set the marker positions so that they track their corresponding characters.
        if (tag == "Player" && player != null)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
        else if (tag == "Blotty" && blotty != null)
        {
            transform.position = new Vector3(blotty.transform.position.x, transform.position.y, blotty.transform.position.z);
        }
        else if (tag == "Winky" && winky != null)
        {
            transform.position = new Vector3(winky.transform.position.x, transform.position.y, winky.transform.position.z);
        }
        else if (tag == "Magenty" && magenty != null)
        {
            transform.position = new Vector3(magenty.transform.position.x, transform.position.y, magenty.transform.position.z);
        }
        else if (tag == "Bonnie" && bonnie != null)
        {
            transform.position = new Vector3(bonnie.transform.position.x, transform.position.y, bonnie.transform.position.z);
        }
        // If the character has been destroyed, turn off the marker.
        else
        {
            icon.GetComponent<MeshRenderer>().enabled = false;
            myLight.GetComponent<Light>().enabled = false;
        }
    }
}
