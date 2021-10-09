using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text mainText;

    private PlayerController player;
    private int playerNum;

    private PhotonView view;

    #region Properties
    public PlayerController Player 
    {
        get 
        {
            return player;
        }
        set 
        {
            player = value;
        }
    }
    public int PlayerNum 
    {
        set 
        {
            playerNum = value;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Start Game
    /// <summary>
    /// Method that handles any necessary actions when starting the FIRST round
    /// </summary>
    public void StartGame() 
    {
        ///RPC call to start the countdown timer
        view.RPC("StartTimerRPC", RpcTarget.All);
    }

    /// <summary>
    /// RPC call that starts the countdown timer
    /// </summary>
    [PunRPC]
    public void StartTimerRPC() 
    {
        StartTimer startTimer = mainText.GetComponent<StartTimer>();
        startTimer.Player = player;
        startTimer.StartCoroutine(startTimer.StartCountdown());
    }
    #endregion

    /// <summary>
    /// Ends the round
    /// </summary>
    public void EndRound() 
    {
        if (playerNum == 1)
        {
            view.RPC("EndRoundTextRPC", RpcTarget.All, "Two");
        }
        else 
        {
            view.RPC("EndRoundTextRPC", RpcTarget.All, "One");
        }
    }

    /// <summary>
    /// RPC Method that displays what player won the round on the main text
    /// </summary>
    /// <param name="playerNum"></param>
    [PunRPC]
    public void EndRoundTextRPC(string playerNum) 
    {
        mainText.text = "Player " + playerNum + " Wins!";
    }
}
