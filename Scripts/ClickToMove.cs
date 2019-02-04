///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: ClickToMove.cs    								       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 22 / 02 / 2018						   /\\\
///\     	    Last entry  - 27 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : Script for controlling the player movement.            /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ClickToMove : MonoBehaviour
{
    public GameObject blottySoul;
    public GameObject winkySoul;
    public GameObject magentySoul;
    public GameObject bonnieSoul;

    private RaycastHit hit;
    
    void FixedUpdate()
    {
        // When LMB is held, cast a ray from camera to mouse position. Set this position as the target for
        // the player's nav mesh agent to walk to.
        if (Input.GetButton("Walk") && !MapGenerationController.hasWonGame && !MapGenerationController.hasLostGame)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                GetComponent<NavMeshAgent>().destination = hit.point;
            }
        }
    }    
}
