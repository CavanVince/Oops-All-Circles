using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnOne;
    [SerializeField] private Transform spawnTwo;

    private Vector2 spawnPosition;

    private void Start()
    {
        //Determine how many players are in the lobby and then spawn them at one of the spawn positions
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            spawnPosition = spawnOne.position;
        }
        else 
        {
            spawnPosition = spawnTwo.position;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
