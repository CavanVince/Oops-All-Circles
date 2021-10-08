using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnOne;
    [SerializeField] private Transform spawnTwo;
    [SerializeField] private StartTimer startTimer;

    private Vector2 spawnPosition;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

        //Determine how many players are in the lobby and then spawn them at one of the spawn positions
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            spawnPosition = spawnOne.position;
        }
        else
        {
            spawnPosition = spawnTwo.position;
            view.RPC("CountdownRPC", RpcTarget.All);
        }
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity).GetComponent<PlayerController>();
    }

    /// <summary>
    /// RPC call to start the countdown function
    /// </summary>
    [PunRPC]
    void CountdownRPC()
    {
        startTimer.StartCoroutine(startTimer.StartCountdown());
    }
}

