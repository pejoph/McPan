///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: MyCameraController.cs    						       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 22 / 02 / 2018						   /\\\
///\     	    Last entry  - 22 / 02 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the game cam.                   /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    public GameObject player;

    void LateUpdate()
    {
        // Camera tracks player but motion is smooth, not fixed.
        transform.position += (player.transform.position - transform.position) / 20.0f;
    }
}
