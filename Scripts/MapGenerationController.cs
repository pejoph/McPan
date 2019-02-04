///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\
///\                                                                   /\\\
///\  Filename: MapGenerationController.cs						       /\\\
///\  																   /\\\
///\  Author  : Peter Phillips										   /\\\
///\     															   /\\\
///\  Date    : First entry - 12 / 03 / 2018						   /\\\
///\     	    Last entry  - 28 / 05 / 2018						   /\\\
///\                                                                   /\\\
///\  Brief   : The main game controlling script. Spawns everything.   /\\\
///             Sets everything's serialized objects. Controls win     /\\\
///             lose conditions.                                       /\\\
///\                                                                   /\\\
///\=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=/\\\



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct Room
{
    public GameObject prefab;
    public int area;
}

public class MapGenerationController : MonoBehaviour
{
    [SerializeField] private GameObject floorTile;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Material Test;
    [SerializeField] private Material Test1;
    [SerializeField] private Material Test2;
    [SerializeField] private Material blottyMat;
    [SerializeField] private Material winkyMat;
    [SerializeField] private Material magentyMat;
    [SerializeField] private Material bonnieMat;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject gameCam;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject raycastTarget;
    [SerializeField] private GameObject particleEffects;
    [SerializeField] private GameObject directionalLight;
    [SerializeField] private GameObject playerMarker;
    [SerializeField] private GameObject ghostMarker;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject inGameUI;
   
    public static List<Room> listOfRoomsCopy = new List<Room>();
    public static bool hasWonGame = false;
    public static bool hasLostGame = false;
    public static float batteryLife = 300;

    private int randomDir;
    private Vector3 spreadVector;
    private List<Room> listOfRooms;
    private List<GameObject> listOfConnectors;
    private int numberOfRooms = 100;
    private bool hasZoomedIn = false;
    
    private void Start()
    {
        // Turn off the scene lighting (Useful when out of play mode.
        directionalLight.GetComponent<Light>().enabled = false;

        // Reset these values (used when restarting the game).
        hasWonGame = false;
        hasLostGame = false;
        batteryLife = 300;
        
        // Create list.
        listOfRooms = new List<Room>();

        for (int i = 0; i < numberOfRooms; i++)
        {
            // Instantiate a room, Randomise the size, calculate the area and add it to the list.
            Room room = new Room();
            room.prefab = Instantiate(floorTile, floorParent);
            room.prefab.transform.localScale = new Vector3(Random.Range(1, 20), 1, Random.Range(1, 20));
            room.area = (int)(room.prefab.transform.localScale.x * room.prefab.transform.localScale.z);
            listOfRooms.Add(room);

            // If the room is within 1m of another room, move it in a random direction.
            while (Physics.OverlapBox(listOfRooms[i].prefab.transform.position, listOfRooms[i].prefab.transform.localScale * 6).Length != 2)
            {
                randomDir = Random.Range(0, 360);
                spreadVector = new Vector3(Mathf.Cos(randomDir * Mathf.PI / 180), 0, Mathf.Sin(randomDir * Mathf.PI / 180));

                listOfRooms[i].prefab.transform.position += 20 * spreadVector;
            }
        }

        // Sort the list of rooms by area.
        listOfRooms.Sort((f1, f2) => f1.area.CompareTo(f2.area));

        // Keep only the 20 largest rooms.
        for (int i = 0; i < 80; i++)
        {
            // The reason we destroy [0] every time is because each time we remove an entry from the list,
            // the next smallest room becomes entry [0].
            Destroy(listOfRooms[0].prefab);
            listOfRooms.Remove(listOfRooms[0]);
        }

        // Sort the list of rooms by z-position.
        listOfRooms.Sort((f2, f1) => f1.prefab.transform.position.z.CompareTo(f2.prefab.transform.position.z));

        listOfRooms[19].prefab.GetComponent<Renderer>().material = Test;
        listOfRooms[0].prefab.GetComponent<Renderer>().material = Test1;
        
        // Update list copy (This saves the room positions becuase the original list 
        // is destroyed in the next stage).
        for (int i = 0; i < 19; i++)
        {
            listOfRoomsCopy.Add(listOfRooms[i]);
        }

        // Create list for connecting paths.
        listOfConnectors = new List<GameObject>();
        
        for (int i = 0; i < 19; i++)
        {
            GameObject connector = Instantiate(floorTile, floorParent);
            connector.GetComponent<Renderer>().material = Test2;

            // Sort room list by distance between rooms and listOfRooms[i] (listOfRooms[i] is now listOfRooms[0] and the closest room is listOfRooms[1]).
            listOfRooms.Sort((f1, f2) => Vector3.Distance(f1.prefab.transform.position, listOfRooms[0].prefab.transform.position).CompareTo(Vector3.Distance(f2.prefab.transform.position, listOfRooms[0].prefab.transform.position)));
            // Place a connecting path half-way between listOfRooms[0] and listOfRooms[1].
            connector.transform.position = (listOfRooms[0].prefab.transform.position + listOfRooms[1].prefab.transform.position) / 2.0f;
            // Move the connecting path down ever so slightly with each connector so that there are no z-fighting issues with the room tiles and with one another.
            connector.transform.position = new Vector3(connector.transform.position.x, connector.transform.position.y - .01f * (i + 1), connector.transform.position.z);
            // Scale the path so that it extends to the two rooms.
            connector.transform.localScale = new Vector3(Vector3.Distance(listOfRooms[0].prefab.transform.position, listOfRooms[1].prefab.transform.position) / 10.0f, 1, 2);
            // Rotate it so that it starts and ends at the centrepoints of the two rooms.
            connector.transform.rotation = Quaternion.Euler(0, 180 / Mathf.PI * Mathf.Asin(((listOfRooms[0].prefab.transform.position.z - listOfRooms[1].prefab.transform.position.z) * (listOfRooms[1].prefab.transform.position.x - listOfRooms[0].prefab.transform.position.x) / Mathf.Abs(listOfRooms[1].prefab.transform.position.x - listOfRooms[0].prefab.transform.position.x)) / Vector3.Distance(listOfRooms[0].prefab.transform.position, listOfRooms[1].prefab.transform.position)), 0);
            // Add this connecting path to a list.
            listOfConnectors.Add(connector);
            // Remove the first room from listOfRooms so that it's not taken into account when making the next path.
            listOfRooms.Remove(listOfRooms[0]);
            // Sort the list of rooms again by z-position.
            listOfRooms.Sort((f2, f1) => f1.prefab.transform.position.z.CompareTo(f2.prefab.transform.position.z));
        }

        // Turn on the mesh collider for the raycast target (It gets in the way of the initial room spawning so it's turned on afterwards).
        raycastTarget.GetComponent<MeshCollider>().enabled = true;

        // Place the player on the starting tile and spawn the gun.
        GameObject Player = Instantiate(player);
        Player.transform.position = new Vector3(listOfRooms[0].prefab.transform.position.x, 5, listOfRooms[0].prefab.transform.position.z);
        GameObject Gun = Instantiate(gun);
        Gun.GetComponent<GunController>().player = Player;

        // Set the camera target.
        gameCam.GetComponent<MyCameraController>().player = Player;

        // Spawn the particle effects.
        GameObject ParticleEffects = Instantiate(particleEffects);
        ParticleEffects.GetComponent<MyCameraController>().player = Player;

        // Spawn the portal.
        GameObject Portal = Instantiate(portal, listOfRoomsCopy[0].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Portal.GetComponent<PortalReferences>().blueBarrier.GetComponent<PortalBarrierController>().player = Player;
        Portal.GetComponent<PortalReferences>().redBarrier.GetComponent<PortalBarrierController>().player = Player;
        Portal.GetComponent<PortalReferences>().pinkBarrier.GetComponent<PortalBarrierController>().player = Player;
        Portal.GetComponent<PortalReferences>().orangeBarrier.GetComponent<PortalBarrierController>().player = Player;
        Portal.GetComponent<PortalReferences>().portal.GetComponent<PortalBarrierController>().player = Player;

        // Spawn the battery.
        GameObject Battery = Instantiate(battery, listOfRoomsCopy[Random.Range(4, 15)].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Battery.GetComponent<BatteryScript>().player = Player;

        // Spawn ghosts.
        // Blotty (Blue) - Standard ghost, easiest to catch.
        // Spawns in the closest range to the player. Each subsequent ghost spawns in a more distant range.
        GameObject Blotty = Instantiate(ghost, listOfRoomsCopy[Random.Range(14, 18)].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Blotty.tag = "Blotty";
        Blotty.GetComponent<Renderer>().material = blottyMat;
        Blotty.GetComponent<RunAwayScript>().player = Player;
        Blotty.GetComponent<RunAwayScript>().gun = Gun;
        Blotty.GetComponent<RunAwayScript>().alertDistance = 150.0f;
        Blotty.GetComponent<RunAwayScript>().movementDistance = 100.0f;
        Blotty.GetComponent<RunAwayScript>().varianceMultiplier = 1.0f;
        Blotty.GetComponent<NavMeshAgent>().speed = 30.0f;
        Blotty.GetComponent<NavMeshAgent>().angularSpeed = 80.0f;
        Blotty.GetComponent<NavMeshAgent>().acceleration = 60.0f;
        // Winky (Red) - Not very alert, faster than the player but darts around erratically 
        // and has low acceleration and turning speed so can be caught.
        GameObject Winky = Instantiate(ghost, listOfRoomsCopy[Random.Range(10, 15)].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Winky.tag = "Winky";
        Winky.GetComponent<Renderer>().material = winkyMat;
        Winky.GetComponent<RunAwayScript>().player = Player;
        Winky.GetComponent<RunAwayScript>().gun = Gun;
        Winky.GetComponent<RunAwayScript>().alertDistance = 75.0f;
        Winky.GetComponent<RunAwayScript>().movementDistance = 100.0f;
        Winky.GetComponent<RunAwayScript>().varianceMultiplier = 4.0f;
        Winky.GetComponent<NavMeshAgent>().speed = 50.0f;
        Winky.GetComponent<NavMeshAgent>().angularSpeed = 25.0f;
        Winky.GetComponent<NavMeshAgent>().acceleration = 50.0f;
        // Magenty (Pink) - Not very alert, Same speed as the player, fast turning speed,
        // can be cornered due to low movement distance.
        GameObject Magenty = Instantiate(ghost, listOfRoomsCopy[Random.Range(6, 12)].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Magenty.tag = "Magenty";
        Magenty.GetComponent<Renderer>().material = magentyMat;
        Magenty.GetComponent<RunAwayScript>().player = Player;
        Magenty.GetComponent<RunAwayScript>().gun = Gun;
        Magenty.GetComponent<RunAwayScript>().alertDistance = 100.0f;
        Magenty.GetComponent<RunAwayScript>().movementDistance = 75.0f;
        Magenty.GetComponent<RunAwayScript>().varianceMultiplier = 2.0f;
        Magenty.GetComponent<NavMeshAgent>().speed = 40.0f;
        Magenty.GetComponent<NavMeshAgent>().angularSpeed = 120.0f;
        Magenty.GetComponent<NavMeshAgent>().acceleration = 80.0f;
        // Bonnie (Orange) - Very alert, can see the player across most of the map,
        // slow movement speed, high acceleration and turning speed, high movement distance, 
        // normally found at the edge of the map.
        GameObject Bonnie = Instantiate(ghost, listOfRoomsCopy[Random.Range(1, 8)].prefab.transform.position, Quaternion.Euler(0, 0, 0));
        Bonnie.tag = "Bonnie";
        Bonnie.GetComponent<Renderer>().material = bonnieMat;
        Bonnie.GetComponent<RunAwayScript>().player = Player;
        Bonnie.GetComponent<RunAwayScript>().gun = Gun;
        Bonnie.GetComponent<RunAwayScript>().alertDistance = 1000.0f;
        Bonnie.GetComponent<RunAwayScript>().movementDistance = 200.0f;
        Bonnie.GetComponent<RunAwayScript>().varianceMultiplier = 1.0f;
        Bonnie.GetComponent<NavMeshAgent>().speed = 25.0f;
        Bonnie.GetComponent<NavMeshAgent>().angularSpeed = 400.0f;
        Bonnie.GetComponent<NavMeshAgent>().acceleration = 100.0f;

        // Spawn markers.
        // Player
        GameObject PlayerMarker = Instantiate(playerMarker);
        PlayerMarker.GetComponent<MiniMapController>().player = Player;
        PlayerMarker.tag = "Player";
        // Blotty.
        GameObject BlottyMarker = Instantiate(ghostMarker);
        BlottyMarker.GetComponent<MiniMapController>().player = Player;
        BlottyMarker.GetComponent<MiniMapController>().blotty = Blotty;
        BlottyMarker.tag = "Blotty";
        // Winky.
        GameObject WinkyMarker = Instantiate(ghostMarker);
        WinkyMarker.GetComponent<MiniMapController>().player = Player;
        WinkyMarker.GetComponent<MiniMapController>().winky = Winky;
        WinkyMarker.tag = "Winky";
        // Magenty.
        GameObject MagentyMarker = Instantiate(ghostMarker);
        MagentyMarker.GetComponent<MiniMapController>().player = Player;
        MagentyMarker.GetComponent<MiniMapController>().magenty = Magenty;
        MagentyMarker.tag = "Magenty";
        // Bonnie.
        GameObject BonnieMarker = Instantiate(ghostMarker);
        BonnieMarker.GetComponent<MiniMapController>().player = Player;
        BonnieMarker.GetComponent<MiniMapController>().bonnie = Bonnie;
        BonnieMarker.tag = "Bonnie";

        // Set ghost markers.
        Magenty.GetComponent<RunAwayScript>().blottyMarker = BlottyMarker;
        Magenty.GetComponent<RunAwayScript>().winkyMarker = WinkyMarker;
        Magenty.GetComponent<RunAwayScript>().magentyMarker = MagentyMarker;
        Magenty.GetComponent<RunAwayScript>().bonnieMarker = BonnieMarker;
    }

    private void Update()
    {
        if (hasWonGame)
        {
            if (mainCam.GetComponent<Camera>().fieldOfView < 178)
            {
                if (hasZoomedIn)
                {
                    // Zoom out.
                    mainCam.GetComponent<Camera>().fieldOfView += 2;
                }
                else
                {
                    // Zoom in.
                    mainCam.GetComponent<Camera>().fieldOfView -= 1;
                }
            }

            if (mainCam.GetComponent<Camera>().fieldOfView <= 5)
            {
                hasZoomedIn = true;
            }
        }


        if (batteryLife > 300)
        {
            batteryLife = 300;
        }
        else if (batteryLife <= 0)
        {
            hasLostGame = true;
        }
        else
        {
            batteryLife -= Time.deltaTime;
        }
    }
}
