using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnOne;
    [SerializeField] private Transform spawnTwo;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        //Determine how many players are in the lobby and then spawn them at one of the spawn positions
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Vector2 spawnPosition = spawnOne.position;

            //Save the player object and number in the game manager so that it can manipulate it later
            gameManager.Player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity).GetComponent<PlayerController>().GetComponent<PlayerController>();
            gameManager.Player.enabled = false;
            gameManager.PlayerNum = 1;
        }
        else
        {
            Vector2 spawnPosition = spawnTwo.position;

            //Save the player object and number in the game manager so that it can manipulate it later
            gameManager.Player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity).GetComponent<PlayerController>().GetComponent<PlayerController>();
            gameManager.Player.enabled = false;
            gameManager.PlayerNum = 2;

            //Tell the game manager to start the game
            gameManager.StartGame();
        }
    }
}

