///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: BatteryScript.cs   								       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 28 / 05 / 2018						   /\\\
///\     	    Last entry  - 28 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the battery mechanics as well   /\\\
///             as the level-up messages.                              /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    public GameObject player;
    public InGameUIScript uiScript;

    private float distance;
    private bool isBatteryPickedUp = false;
    private float timer = 0;
    private float blottyTimer = 0.0f;
    private float winkyTimer = 0.0f;
    private float magentyTimer = 0.0f;
    private float bonnieTimer = 0.0f;

    private void Start()
    {
        uiScript = GetComponent<InGameUIScript>();
    }

    void Update()
    {        
        // Rotate battery.
        rotator.transform.rotation = Quaternion.Euler(0, Time.time * 180, 0);

        // Calculate distance to player.
        distance = Vector3.Distance(player.transform.position, transform.position);

        // Pick up battery if close enough.
        if (distance < 10)
        {
            // Teleport battery to new location.
            transform.position = MapGenerationController.listOfRoomsCopy[Random.Range(4, 15)].prefab.transform.position;
            // Increase battery life.
            MapGenerationController.batteryLife += 30;
            // State battery is picked up.
            isBatteryPickedUp = true;
        }        

        // Fade the message in.
        if (isBatteryPickedUp)
        {
            timer += Time.deltaTime;
            InGameUIScript.batteryAlpha = timer;
        }
        // Keep message at half opacity for 2 seconds.
        if (timer >= .5 && timer <= 2.5)
        {
            InGameUIScript.batteryAlpha = .5f;
        }
        // Fade out.
        else if (timer > 2.5 && timer < 3)
        {
            InGameUIScript.batteryAlpha = 3.0f - timer;
        }
        // Reset.
        else if (timer >= 3)
        {
            isBatteryPickedUp = false;
            timer = 0;
            InGameUIScript.batteryAlpha = 0;
        }

        // Display message detailing acquired bonus when a ghost is vacuumed.
        // Blotty.
        // Fade the message in.
        if (RunAwayScript.isBlottyDead)
        {
            blottyTimer += Time.deltaTime;
            InGameUIScript.speedAlpha = blottyTimer;
        }
        // Keep message at half opacity for 2 seconds.
        if (blottyTimer >= .5 && blottyTimer <= 4.5)
        {
            InGameUIScript.speedAlpha = .5f;
        }
        // Fade out.
        else if (blottyTimer > 4.5 && blottyTimer < 5)
        {
            InGameUIScript.speedAlpha = 5.0f - blottyTimer;
        }
        // Reset.
        else if (blottyTimer >= 5)
        {
            blottyTimer = 0;
            RunAwayScript.isBlottyDead = false;
            InGameUIScript.speedAlpha = 0;
        }

        // Winky.
        // Fade the message in.
        if (RunAwayScript.isWinkyDead)
        {
            winkyTimer += Time.deltaTime;
            InGameUIScript.torchAngleAlpha = winkyTimer;
        }
        // Keep message at half opacity for 2 seconds.
        if (winkyTimer >= .5 && winkyTimer <= 4.5)
        {
            InGameUIScript.torchAngleAlpha = .5f;
        }
        // Fade out.
        else if (winkyTimer > 4.5 && winkyTimer < 5)
        {
            InGameUIScript.torchAngleAlpha = 5.0f - winkyTimer;
        }
        // Reset.
        else if (winkyTimer >= 5)
        {
            winkyTimer = 0;
            RunAwayScript.isWinkyDead = false;
            InGameUIScript.torchAngleAlpha = 0;
        }

        // Magenty.
        // Fade the message in.
        if (RunAwayScript.isMagentyDead)
        {
            magentyTimer += Time.deltaTime;
            InGameUIScript.spyModeAlpha = magentyTimer;
        }
        // Keep message at half opacity for 2 seconds.
        if (magentyTimer >= .5 && magentyTimer <= 4.5)
        {
            InGameUIScript.spyModeAlpha = .5f;
        }
        // Fade out.
        else if (magentyTimer > 4.5 && magentyTimer < 5)
        {
            InGameUIScript.spyModeAlpha = 5.0f - magentyTimer;
        }
        // Reset.
        else if (magentyTimer >= 5)
        {
            magentyTimer = 0;
            RunAwayScript.isMagentyDead = false;
            InGameUIScript.spyModeAlpha = 0;
        }

        // Bonnie.
        // Fade the message in.
        if (RunAwayScript.isBonnieDead)
        {
            bonnieTimer += Time.deltaTime;
            InGameUIScript.rangeAlpha = bonnieTimer;
        }
        // Keep message at half opacity for 2 seconds.
        if (bonnieTimer >= .5 && bonnieTimer <= 4.5)
        {
            InGameUIScript.rangeAlpha = .5f;
        }
        // Fade out.
        else if (bonnieTimer > 4.5 && bonnieTimer < 5)
        {
            InGameUIScript.rangeAlpha = 5.0f - bonnieTimer;
        }
        // Reset.
        else if (bonnieTimer >= 5)
        {
            bonnieTimer = 0;
            RunAwayScript.isBonnieDead = false;
            InGameUIScript.rangeAlpha = 0;
        }
    }
}
