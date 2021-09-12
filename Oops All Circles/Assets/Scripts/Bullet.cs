using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float timer;
    private float wallBounceCounter;
    private GameObject parentPlayer;

    public GameObject ParentPlayer 
    {
        get 
        {
            return parentPlayer;
        }
        set
        {
            parentPlayer = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        wallBounceCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Decrement the timer and possibly destroy the bullet
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
           PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wallBounceCounter--;

        //Determine if the ball hit two walls
        if (wallBounceCounter < 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
