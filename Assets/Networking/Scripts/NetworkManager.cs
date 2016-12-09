using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManager : Photon.PunBehaviour
{
    [SerializeField]
    Camera standbyCamera;
    [SerializeField]
    private Transform[] spawnPoint;
    [SerializeField]
    private GameObject ConnectButtonGO;
    [SerializeField]
    private Text ConnectingText;

    public GameObject Sticks;
    const string VERSION = "v0.0.1";
    public string roomName = "Maze";
    public string playerPrefabName = "Player";
    int positionNumber;
    int newPos;
    bool StartGame = false;
    int numberPlayers;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v0.1");
        Debug.Log("1");
        ConnectingText.text = "Starting connection...";
    }



    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
        //Debug.Log("ok");
        ConnectingText.text = "Lobby Joined...Creating Room";
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }

    //public override void OnJoinedLobby()
    //{
    //    RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
    //    PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    //    Debug.Log("2");
    //}

    public override void OnJoinedRoom()
    {
        ConnectingText.text = "Room joined! Press start!";
        ConnectButtonGO.SetActive(true);
        //PhotonNetwork.Instantiate("Player", spawnPoint[Random.Range(0, spawnPoint.Length)].position, spawnPoint[Random.Range(0, spawnPoint.Length)].rotation, 0);
        //Debug.Log("Pos1 " + pos1 + " Pos2 " + pos2 + " Pos3 " + pos3 + " Pos4 " + pos4);

    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void Update()
    {
        StartGame = ConnectButton.Connect;
        if (StartGame)
        {
            ConnectingText.enabled = false;
            //Deactivate the Connection button
            ConnectButtonGO.SetActive(false);
            //Run CheckPlayer function to determine how many players there are in the room already
            CheckPlayers();
            standbyCamera.enabled = false;
            //Debug.Log("Players in the room after start pressed " + PhotonNetwork.countOfPlayers.ToString());

            //Determine which spawn point to use based on the number of player
            if (numberPlayers == 1)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[0].position, spawnPoint[0].rotation, 0);
                numberPlayers = 2;
                ConnectButton.Connect = false;
            }

            else if (numberPlayers == 2)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[1].position, spawnPoint[1].rotation, 0);
                numberPlayers = 3;
                ConnectButton.Connect = false;
            }
            else if (numberPlayers == 3)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[2].position, spawnPoint[2].rotation, 0);
                numberPlayers = 4;
                ConnectButton.Connect = false;
            }
            else if (numberPlayers == 4)
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[3].position, spawnPoint[3].rotation, 0);
                numberPlayers = 1;
                ConnectButton.Connect = false;
            }
        }
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.countOfPlayers;
        //if the number of player is heigher than the number of spawnpoint in the game (in this case 4),
        //spawn the players in round order
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > 4)
            {
                numberPlayers -= 4;
            }

        }
    }
 