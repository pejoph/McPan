///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: RunAwayScript.cs    							       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 27 / 05 / 2018						   /\\\
///\     	    Last entry  - 28 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling ghost mechanics.                /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayScript : MonoBehaviour
{
    public GameObject player;
    public GameObject gun;
    public float alertDistance = 100.0f;
    public float movementDistance = 100.0f;
    public float varianceMultiplier = 1.0f;
    public GameObject blottyMarker;
    public GameObject winkyMarker;
    public GameObject magentyMarker;
    public GameObject bonnieMarker;

    public static bool isBlottyDead = false;
    public static bool isWinkyDead = false;
    public static bool isMagentyDead = false;
    public static bool isBonnieDead = false;

    private float varianceTimer = 1.0f;
    private float movementVariance = 0.0f;
    private float playerDistance;
    private Vector3 escapeDirection;

    private void Start()
    {
        isBlottyDead = false;
        isWinkyDead = false;
        isMagentyDead = false;
        isBonnieDead = false;
    }

    void Update ()
    {
        // Calculate distance between player and ghost.
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        // Calculate direction from player to ghost (This is the average direction the ghost will travel to escape).
        escapeDirection = transform.position - player.transform.position;

        // Every second that the player is in the alert range of the ghost, the ghost's direction will change slightly to give it some unpredicatability and make it harder to corner.
        if (varianceTimer >= 1)
        {
            movementVariance = Random.Range(-1.0f, 1.0f);
            varianceTimer = 0.0f;
        }
        else
        {
            varianceTimer += Time.deltaTime;
        }        

        // When the player is near the ghost, set the ghost's destination 100m (or whatever the movement distance is) in the opposite direction (plus a random tangent multiplied by the variance multiplier).
		if (playerDistance <= alertDistance)
        {
            GetComponent<NavMeshAgent>().destination = transform.position + movementDistance * Vector3.Normalize(escapeDirection + varianceMultiplier * movementVariance * Vector3.Cross(Vector3.up, escapeDirection));
        }
        // When the player is not near the ghost, set the destination to a random room within the ghost's spawn range.
        // Change destination once previous destination is reached.
        else if (GetComponent<NavMeshAgent>().destination.x == transform.position.x && GetComponent<NavMeshAgent>().destination.z == transform.position.z)
        {
            if (tag == "Blotty")
            {
                GetComponent<NavMeshAgent>().destination = MapGenerationController.listOfRoomsCopy[Random.Range(14, 18)].prefab.transform.position;
            }
            else if (tag == "Winky")
            {
                GetComponent<NavMeshAgent>().destination = MapGenerationController.listOfRoomsCopy[Random.Range(10, 15)].prefab.transform.position;
            }
            else if (tag == "Magenty")
            {
                GetComponent<NavMeshAgent>().destination = MapGenerationController.listOfRoomsCopy[Random.Range(6, 12)].prefab.transform.position;
            }
            else if (tag == "Bonnie")
            {
                GetComponent<NavMeshAgent>().destination = MapGenerationController.listOfRoomsCopy[Random.Range(1, 8)].prefab.transform.position;
            }
        }

        // Replenishes size up to the maximum.
        if (transform.localScale.x > 7.5)
        {
            transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
        }
        else if (transform.localScale.x < 7.5)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime, transform.localScale.y + Time.deltaTime, transform.localScale.z + Time.deltaTime);
        }

        // Ghost is destroyed if size reaches 2.5.
        if (transform.localScale.x < 2.5)
        {
            Destroy(gameObject);

            // When each ghost is destroyed, the corresponding soul appears on the player's backpack.
            if (tag == "Blotty")
            {
                player.GetComponent<ClickToMove>().blottySoul.GetComponent<MeshRenderer>().enabled = true;
                player.GetComponent<ClickToMove>().blottySoul.GetComponent<Behaviour>().enabled = true;

                // Increase player speed by 20%.
                player.GetComponent<NavMeshAgent>().speed = 48;
                // Flip this bool to trigger a message.
                isBlottyDead = true;
            }
            else if (tag == "Winky")
            {
                player.GetComponent<ClickToMove>().winkySoul.GetComponent<MeshRenderer>().enabled = true;
                player.GetComponent<ClickToMove>().winkySoul.GetComponent<Behaviour>().enabled = true;

                // Increase torch angle by 20%.
                gun.GetComponent<GunController>().muzzle.GetComponent<Light>().spotAngle = 120;
                // Flip this bool to trigger a message.
                isWinkyDead = true;
            }
            else if (tag == "Magenty")
            {
                player.GetComponent<ClickToMove>().magentySoul.GetComponent<MeshRenderer>().enabled = true;
                player.GetComponent<ClickToMove>().magentySoul.GetComponent<Behaviour>().enabled = true;

                // Add ghost locations to minimap.
                blottyMarker.GetComponent<MiniMapController>().icon.GetComponent<MeshRenderer>().enabled = true;
                blottyMarker.GetComponent<MiniMapController>().myLight.GetComponent<Light>().enabled = true;
                winkyMarker.GetComponent<MiniMapController>().icon.GetComponent<MeshRenderer>().enabled = true;
                winkyMarker.GetComponent<MiniMapController>().myLight.GetComponent<Light>().enabled = true;
                magentyMarker.GetComponent<MiniMapController>().icon.GetComponent<MeshRenderer>().enabled = true;
                magentyMarker.GetComponent<MiniMapController>().myLight.GetComponent<Light>().enabled = true;
                bonnieMarker.GetComponent<MiniMapController>().icon.GetComponent<MeshRenderer>().enabled = true;
                bonnieMarker.GetComponent<MiniMapController>().myLight.GetComponent<Light>().enabled = true;
                // Flip this bool to trigger a message.
                isMagentyDead = true;
            }
            else if (tag == "Bonnie")
            {
                player.GetComponent<ClickToMove>().bonnieSoul.GetComponent<MeshRenderer>().enabled = true;
                player.GetComponent<ClickToMove>().bonnieSoul.GetComponent<Behaviour>().enabled = true;

                //Increase vacuum range by 20%.
                gun.GetComponent<GunController>().muzzle.transform.localScale = new Vector3(.5f, .5f, 4.87f);
                // Flip this bool to trigger a message.
                isBonnieDead = true;
            }
        }        
    }

    // If the player is shooting the ghost and within range, reduce the size of 
    // the ghost (this represents the ghost being sucked into the vacuum).
    // It takes 2.5 seconds to suck up each ghost.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Vacuum")
        {
            transform.localScale = new Vector3(transform.localScale.x - 3 * Time.deltaTime, transform.localScale.y - 3 * Time.deltaTime, transform.localScale.z - 3 * Time.deltaTime);
        }
    }
}
