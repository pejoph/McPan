///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: GunController.cs   								       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 27 / 05 / 2018						   /\\\
///\     	    Last entry  - 27 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the gun mechanics.              /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject player;
    public GameObject barrel;
    public GameObject muzzle;

    private RaycastHit hit;
    private int layerMask = 1 << 8;     // Sets layer mask to layer 8.
    
    private void Update()
    {
        // When RMB is held down, play the particle effects and turn on the 
        // box collider (this is what interacts with the ghosts).
        if (Input.GetButton("Fire"))
        {
            muzzle.GetComponent<ParticleSystem>().Play();
            muzzle.GetComponent<BoxCollider>().enabled = true;
        }
        // Otherwise, don't.
        else
        {
            muzzle.GetComponent<ParticleSystem>().Stop();
            muzzle.GetComponent<ParticleSystem>().Clear();
            muzzle.GetComponent<BoxCollider>().enabled = false;
        }

        // light flashes off and on to indicate 30 seconds battery life remaining.
        if (MapGenerationController.batteryLife <= 30.1 && MapGenerationController.batteryLife >= 29.9)
        {
            muzzle.GetComponent<Light>().enabled = false;
        }
        else
        {
            muzzle.GetComponent<Light>().enabled = true;
        }

        // Light turns off when time runs out.
        if (MapGenerationController.hasLostGame)
        {
            muzzle.GetComponent<Light>().enabled = false;
        }
    }

    void LateUpdate()
    {
        // Attach gun to player position.
        transform.position = player.transform.position;

        // Sends a raycast to an invisible collider that covers the map.
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            // Where the ray hits is where the gun points.
            barrel.transform.LookAt(hit.point);
            transform.LookAt(hit.point);
        }        
    }
}
