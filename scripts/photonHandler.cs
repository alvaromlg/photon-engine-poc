using UnityEngine;
using System.Collections;

public class photonHandler : MonoBehaviour
{
	/// <summary>
	/// This script should live within a GameObject and never be destroyed.
	/// It will handle Photon Framework network events, create rooms, join players to rooms, etc
    /// </summary>

    //  player prefab
    public GameObject mainPlayer;
    public string versionName = "0.1";

	// On awake we connect to photon
    private void Awake()
    {
		// connect to photon
        PhotonNetwork.ConnectUsingSettings(versionName);
		// dont destroy this script (game object)
        DontDestroyOnLoad(this.transform);
        Debug.Log("connected to server");
    }

	// On connected to photon we join a room or
	// create one if no room available
	// Everyone will connect to the same room "server"
    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        Debug.Log("We are connected to master");

        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 20;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("server", roomOptions, TypedLobby.Default);
    }

	// This event is useful to display a list of rooms
    private void OnJoinedLobby()
    {

        Debug.Log("Connected");
    }

	// Joined room, now we can really spawn the player
    private void OnJoinedRoom()
    {
        spawnPlayer();
    }

	// Also if it's a client net issue we could useful OnConnectionFail
    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("You have been disconnected");
    }

    private void OnFailedToConnectToPhoton()
    {
        Debug.Log("Lost connection to server");
    }

    private void spawnPlayer()
    {
        // respawn player
        GameObject player = PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);

        // Get main camera
        GameObject playerCamera = GameObject.FindWithTag("MainCamera");

        // follow player script
        CameraController followScript = playerCamera.GetComponent("CameraController") as CameraController;
        followScript.target = player;

        // assign cam to the player to calculate player camera mouse position (used for attacks)
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.plCam = playerCamera;
    }
}