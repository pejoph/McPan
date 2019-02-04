///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: PortalBarrierController.cs    					       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 28 / 05 / 2018						   /\\\
///\     	    Last entry  - 28 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for destroying the barriers at the end of       /\\\
///             the level.                                             /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBarrierController : MonoBehaviour
{
    public GameObject player;

    private float distance;

	void Update ()
    {
        // Calculate distance between player and portals.
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < 25)
        {
            // If player is withing range and has caught the relevant ghost, destroy the barrier and remove
            // the soul from the player's backpack.
            if (tag == "Blotty" && player.GetComponent<ClickToMove>().blottySoul.GetComponent<MeshRenderer>().isVisible)
            {
                player.GetComponent<ClickToMove>().blottySoul.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<ClickToMove>().blottySoul.GetComponent<Behaviour>().enabled = false;

                Destroy(gameObject);
            }
            else if (tag == "Winky" && player.GetComponent<ClickToMove>().winkySoul.GetComponent<MeshRenderer>().isVisible)
            {
                player.GetComponent<ClickToMove>().winkySoul.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<ClickToMove>().winkySoul.GetComponent<Behaviour>().enabled = false;

                Destroy(gameObject);
            }
            else if (tag == "Magenty" && player.GetComponent<ClickToMove>().magentySoul.GetComponent<MeshRenderer>().isVisible)
            {
                player.GetComponent<ClickToMove>().magentySoul.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<ClickToMove>().magentySoul.GetComponent<Behaviour>().enabled = false;

                Destroy(gameObject);
            }
            else if (tag == "Bonnie" && player.GetComponent<ClickToMove>().bonnieSoul.GetComponent<MeshRenderer>().isVisible)
            {
                player.GetComponent<ClickToMove>().bonnieSoul.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<ClickToMove>().bonnieSoul.GetComponent<Behaviour>().enabled = false;

                Destroy(gameObject);
            }
            // Once barriers are down, player can enter the portal to win the game.
            else if (tag == "Portal" && distance < 12.5)
            {
                Debug.Log("you win!!");
                MapGenerationController.hasWonGame = true;
            }
        }
	}
}
